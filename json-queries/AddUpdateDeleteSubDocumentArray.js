db.document.insert( 
    { 
        name: "doc1", 
        fields: 
        [
            {
                id: "field1",
                value: "val1"
            }
        ]
        
    } 
)

db.document.find( { name: "doc1" } )

db.document.updateOne(
    { name: "doc1" },
    [
        {
            $set: 
            {
                fields:
                {
                    $reduce:
                    {
                        input: '$fields',
                        initialValue  : [],
                        in:
                        {
                            $switch :
                            {
                                branches  : 
                                [
                                    // add if does not exist
                                    {
                                        case: 
                                        {
                                            /*$or: [
                                                { $eq : [ "$$this.id", "field2" ] },
                                                ]*/
                                            "$$this.id": { $exists: true, $nin: [ "field2" ] }
                                        }
                                        then:
                                        {
                                            $concatArrays: [ "$$value", [
                                                "$$this",
                                                {
                                                    id: "field2",
                                                    value: "val2"
                                                }
                                                ]
                                            ]
                                        }
                                    },
                                ],
                                // by default do nothing
                                default : { $concatArrays : [ "$$value", [ "$$this" ] ] },
                            }
                        }
                    }
                }
            }
        }
    ]
)

db.document.updateOne(
    { name: "doc1" },
    [
        {
            $set: 
            {
                fields:
                {
                    $reduce:
                    {
                        input: '$fields',
                        initialValue  : [],
                        in:
                        {
                            $cond:
                            {
                                if: 
                                {
                                    $not: 
                                    {
                                        $in: ["$$this.id", ["field2"]]
                                    }
                                },
                                then:
                                {
                                    $concatArrays: [ "$$value", [
                                        "$$this",
                                        {
                                            id: "field2",
                                            value: "val2"
                                        }
                                        ]
                                    ]
                                }
                                else:{
                                    $concatArrays : [ "$$value", [ "$$this" ] ]
                                }
                            }
                        }
                    }
                }
            }
        }
    ]
)

db.document.insert( 
    { 
        name: "doc1", 
        fields: 
        [
            {
                id: "field1",
                value: "val1"
            }
        ]
        
    } 
)

db.document.find( { name: "doc1" } )
// https://stackoverflow.com/questions/61004259/add-an-element-to-an-array-if-it-exists-dont-add-it-if-it-exists-update-it
db.document.updateOne(
    { name: "doc1" },
    [
        {
            $set: 
            {
                fields:
                {
                    $reduce:
                    {
                        input: '$fields',
                        initialValue  : 
                        [
                            {
                                id: "field2",
                                value: "val3"
                            }
                        ],
                        in:
                        {
                            $cond:
                            [
                                { $in: ["$$this.id", "$$value.id"] }, // Check id exists in 'fields' array
                                ["$$this"], // If YES, return input
                                { $concatArrays: ["$$value", ["$$this"]] }, // If YES, do nothing
                            ]
                        }
                    }
                }
            }
        }
    ]
)


// this will add nothing becasue $addToSet compares all object fields and adds only if such object does not exist in the array
db.document.updateOne(
    { name: "doc1" },
    { $addToSet: 
        {
            fields:
            {
                id: "field1",
                value: "val1" 
            }
        }
    }
)

// https://stackoverflow.com/questions/72545681/mongodb-use-addtoset-with-conditions
// it looks that condition can be used only when $addToSet is used in the aggregation pipeline
db.document.updateOne(
    { name: "doc1" },
    { $addToSet: 
        {
            $cond:
            {
                if:
                {
                    $eq: ["$fields.$id", "field1"]
                },
                then:
                {
                    fields:
                    {
                        id: "field1",
                        value: "val1" 
                    }
                },
                else:
                {
                    fields:
                    {
                        id: "field1",
                        value: "val1" 
                    }                    
                }
                
            }
        }
    }
)

// this will add field2 because it does not exist
db.document.updateOne(
    { name: "doc1", "fields.id": { $ne: "field2" } },
    { $push: 
        {
            fields:
            {
                id: "field2",
                value: "val1" 
            }
        }
    }
)