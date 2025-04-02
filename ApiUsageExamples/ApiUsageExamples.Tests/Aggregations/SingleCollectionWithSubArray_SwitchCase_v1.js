[
    {
        $match: {
            _id: ObjectId("67ed67d7d97e178ccd243a32")
        }
    },
    {
        $unwind: "$Documents"
    },
    {
        $group: {
            _id: null,
            statuses: {
                $addToSet: "$Documents.Status"
            }
        }
    },
    {
        $project: {
            aggregatedStatus: {
                $switch: {
                    branches: [
                        {
                            case: {
                                $and: [
                                    {
                                        $in: [
                                            "Processing",
                                            "$statuses"
                                        ]
                                    }
                                ]
                            },
                            then: "Processing"
                        },
                        {
                            case: {
                                $and: [
                                    {
                                        $not: {
                                            $in: [
                                                "Processing",
                                                "$statuses"
                                            ]
                                        }
                                    },
                                    {
                                        $in: ["Failed", "$statuses"]
                                    }
                                ]
                            },
                            then: "Failed"
                        },
                        {
                            case: {
                                $and: [
                                    {
                                        $not: {
                                            $in: [
                                                "Processing",
                                                "$statuses"
                                            ]
                                        }
                                    },
                                    {
                                        $not: {
                                            $in: ["Failed", "$statuses"]
                                        }
                                    }
                                ]
                            },
                            then: "Succeeded"
                        }
                    ],
                    default: null
                }
            }
        }
    },
    {
        $project: {
            _id: 0,
            aggregatedStatus: 1
        }
    }
]