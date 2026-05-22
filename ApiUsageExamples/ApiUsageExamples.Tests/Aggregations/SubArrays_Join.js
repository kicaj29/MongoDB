[
    {
        $match:
        /**
         * query: The query in MQL.
         */
        {
            _id: ObjectId("6a10027687447ca218c948e0")
        }
    },
    {
        $unwind:
        /**
         * path: Path to the array field.
         * includeArrayIndex: Optional name for index.
         * preserveNullAndEmptyArrays: Optional
         *   toggle to unwind null and empty values.
         */
        {
            path: "$Documents"
        }
    },
    {
        $match:
        /**
         * query: The query in MQL.
         */
        {
            "Documents.ID": ObjectId(
                "6a10027687447ca218c948e2"
            )
        }
    },
    {
        $project:
        /**
         * specifications: The fields to
         *   include or exclude.
         */
        {
            DocId: "$Documents.ID",
            ClassId: "$Documents.ClassId",
            ClassName: {
                $let: {
                    vars: {
                        this: {
                            $arrayElemAt: [
                                {
                                    $filter: {
                                        input: "$ClassDefinitions",
                                        as: "c",
                                        cond: {
                                            $eq: [
                                                "$$c.ID",
                                                "$Documents.ClassId"
                                            ]
                                        }
                                    }
                                },
                                0
                            ]
                        }
                    },
                    in: "$$this.Name"
                }
            },
            _id: 0
        }
    }
]