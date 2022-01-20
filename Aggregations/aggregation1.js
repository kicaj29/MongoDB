db.getCollection('batches').aggregate(
[
  {
    '$match': {
      'ID': 'c4de2759-6580-42e6-a3e5-aabd9cf3fc8b'
    }
  },
  {   
    '$lookup': {
      'from': 'documents', 
      'localField': 'DocumentIDs', 
      'foreignField': 'ID', 
      'as': 'DocumentData'
    }
  },
  {
    '$project': {
        'actionType' : '$DocumentData.DocumentStatus.ActionType',
        'status' : '$DocumentData.DocumentStatus.Status'
    }
  }
])


db.getCollection('batches').aggregate(
[
  {
    '$match': {
      'ID': 'c4de2759-6580-42e6-a3e5-aabd9cf3fc8b'
    }
  },
  {   
    '$lookup': {
      'from': 'documents', 
      'localField': 'DocumentIDs', 
      'foreignField': 'ID', 
      'as': 'DocumentData'
    }
  },
  {
    '$project': {
        'DocumentStatus' : '$DocumentData.DocumentStatus'
    }
  }
])


