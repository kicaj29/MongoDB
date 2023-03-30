db.getCollection('batches').aggregate(
[
  {
    '$match': {
      'ID': '5cf35264-bd41-4485-a55a-19f5fb1c3f21'
    }
  },
  {
    '$unwind': {
      'path': '$DocumentIDs'
    }
  },
  {
    '$project': {
      'documentId': '$DocumentIDs'
    }  
  },
  {   
    '$lookup': {
      'from': 'documents', 
      'localField': 'documentId', 
      'foreignField': 'ID', 
      'as': 'join_results'
    }
  }, 
  {
    '$unwind': {
      'path': '$join_results'
    }
  },
  {
    '$project': {
      '_id': 0
      'actionType': '$join_results.DocumentStatus.ActionType', 
      'status': '$join_results.DocumentStatus.Status'
    }
  }
]    
)


