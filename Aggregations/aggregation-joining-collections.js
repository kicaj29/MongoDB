[
  {
    '$match': {
      'DocumentStatus': {
        '$ne': null
      }
    }
  }, {
    '$project': {
      'documentId': '$ID'
    }
  }, {
    '$lookup': {
      'from': 'batches', 
      'localField': 'documentId', 
      'foreignField': 'DocumentIDs', 
      'as': 'join_results'
    }
  }, {
    '$unwind': {
      'path': '$join_results'
    }
  }, {
    '$project': {
      'batchID': '$join_results.ID'
    }
  }, {
    '$count': 'batchesCount'
  }
]