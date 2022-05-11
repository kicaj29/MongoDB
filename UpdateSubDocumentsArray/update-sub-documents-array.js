db.orchestrationBatch.find(
    {
    })
   .projection({})
   .sort({_id:-1})
   .limit(100)
   
/*-----------------------------*/

db.orchestrationBatch.find(
    {
        "ID": "0aee8845-e1e0-492f-9faa-5b868e9cd349"
    })
   .projection({})
   .sort({_id:-1})
   .limit(100)
   

/*-----------------------------*/

// https://www.mongodb.com/docs/manual/reference/operator/update/positional-filtered/

mongoexport --uri "mongodb://localhost" --collection orchestrationBatch --db CAP_ORCHESTRATIONINTEGRATIONTEST --out "D:\\GitHub\\kicaj29\\MongoDB\\UpdateSubDocumentsArray\\data.json"
mongoimport --uri "mongodb://localhost" --file "D:\\GitHub\\kicaj29\\MongoDB\\UpdateSubDocumentsArray\\data.json" --collection orchestrationBatch --db CAP_ORCHESTRATIONINTEGRATIONTEST --drop

db.orchestrationBatch.updateOne(
    {
        "ID": "0aee8845-e1e0-492f-9faa-5b868e9cd349"
    },
    {
        "$set": 
        { 
            "Documents.$[docElement].Actions.$[actionElement]":   {
                                            "ID": "aaaaaaaaa",
                                            "ActionType": "NewActionType",
                                            "Status": "Complete"
                                        }
        }
    },
    {
        "arrayFilters": [ 
            { "docElement.ID": { "$in": ["c04a277b-cc09-44a9-ba93-003a4fe75776", "129dfc33-0a59-416c-b8b6-22ea360de1ea"] }  } ,
            { "actionElement.Status": "Queued"}
            ]
    })
    
    
db.orchestrationBatch.updateOne(
    {
        "ID": "0aee8845-e1e0-492f-9faa-5b868e9cd349"
    },
    {
        "$set": 
        { 
            "Documents.$[docElement].Actions.$[actionElement]":   {
                                            "ID": "aaaaaaaaa",
                                            "ActionType": "NewActionType",
                                            "Status": "Complete"
                                        }
        }
    },
    {
        "arrayFilters": [ 
            { "docElement.ID": { '$in': ['c04a277b-cc09-44a9-ba93-003a4fe75776', '129dfc33-0a59-416c-b8b6-22ea360de1ea'] }  } ,
            { "actionElement.Status": "Queued"}
            ]
    })