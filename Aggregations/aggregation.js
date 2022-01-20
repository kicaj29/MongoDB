db.getCollection('batches').aggregate(
[
  {
    '$match': {
      'ID': 'c4de2759-6580-42e6-a3e5-aabd9cf3fc8b'
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


