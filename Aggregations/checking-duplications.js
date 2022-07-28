[{
    $sort: {
        ID: 1
    }
}, {
    $group: {
        _id: {
            ID: "$ID"
        },
        IDs: {
            $push: "$ID"
        }
    }
}, {
    $project: {
        count: {
            $size: "$IDs"
        }
    }
}, {
    $match: {
        count: {
            $gt: 1
        }
    }
}]