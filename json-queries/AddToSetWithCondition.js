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