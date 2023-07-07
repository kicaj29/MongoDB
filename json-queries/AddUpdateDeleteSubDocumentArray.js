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

db.document.insert( 
    { 
        name: "doc1", 
        fields: 
        [
            {
                id: "field1",
                value: "val1"
            },
            {
                id: "field2",
                value: "val2"
            }
        ]
        
    } 
)

db.document.find( { name: "doc1" } )

// GOOD!!!!
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
                                id: "field3",
                                value: "val3"
                            },
                            {
                                id: "field4",
                                value: "val4"
                            }   
                        ],
                        in:
                        {
                            $switch :
                            {
                                branches  : 
                                [
                                    // remove item from 'fields' array if it does not exist in initialValue array
                                    // removal has to be before add, if it as after add then the removal is not executed!!!
                                    {
                                        case:
                                        {
                                            nor: 
                                            [
                                                { $eq : [ "$$this.id", "field3" ] },
                                                { $eq : [ "$$this.id", "field4" ] },
                                            ]
                                        },
                                        then: "$$value",
                                    }
                                    // add item from initialValue if does no exist in 'fields' array
                                    {
                                        case: 
                                        {
                                            // use $not because $nin is not supported in $switch
                                            $not: 
                                            {
                                                $in: ["$$this.id", "$$value.id"]    
                                            }
                                            //$nin: ["$$this.id", "$$value.id"]
                                            
                                        },
                                        then: { $concatArrays: ["$$value", ["$$this"]] }
                                    },
                                ],
                                // by default do nothting with the current element
                                default : ["$$this"],
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
                        initialValue  :
                        [
                            {
                                id: "field2",
                                value: "val3"
                            }                            
                        ],
                        in:
                        {
                            $switch :
                            {
                                branches  : 
                                [
                                    // add if exists, do nothing
                                    {
                                        case: 
                                        {
                                            $in: ["$$this.id", "$$value.id"]
                                        }
                                        then: ["$$this"]
                                    },
                                ],
                                // by default add
                                default : { $concatArrays: ["$$value", ["$$this"]] },
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
                                { $in: ["$$this.id", "$$value.id"] }, // Check if id exists in 'fields' array
                                ["$$this"], // If YES, return input
                                { $concatArrays: ["$$value", ["$$this"]] }, // If NO, add this element to the array
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