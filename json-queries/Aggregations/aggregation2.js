db.getCollection('batches').aggregate(
[
  {
    '$match': {
      'ID': '4e4d60dd-50a6-4d40-9016-97c3f78747dd'
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
    '$lookup': {
      'from': 'docpages', 
      'localField': 'DocumentData.PageIDs', 
      'foreignField': 'ID', 
      'as': 'PageData'
    }
  },
  {
    '$unwind'  : {
        'path': '$DocumentData',
        'preserveNullAndEmptyArrays': false
    }
  },
  {
    '$unwind'  : {
        'path': '$PageData',
        'preserveNullAndEmptyArrays': false
    }
  },  
  {
    '$project': {
        '_id': 0
        'oldDocActionType' : '$DocumentData.DocumentStatus.ActionType',
        'oldDocStatus' : '$DocumentData.DocumentStatus.ActionStatus',
        'docActionType' : '$DocumentData.ActionStatus.ActionType',
        'docActionStatus' : '$DocumentData.ActionStatus.Status',
        'pageActionType' : '$PageData.ActionStatus.ActionType',
        'pageActionStatus' : '$PageData.ActionStatus.Status',
    }
  }
])


