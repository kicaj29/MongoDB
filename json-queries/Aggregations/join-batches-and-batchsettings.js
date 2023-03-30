[{$project: {
      "_id": {
        "$toString": "$_id"
      },
      "Name": 1,
      "ContentQueueID": 1,
}}, {$lookup: {
  from: 'batchsettings',
  localField: '_id',
  foreignField: 'BatchId',
  as: 'result1'
}}, {$unwind: {
  path: '$result1',
  preserveNullAndEmptyArrays: false
}}, {$project: {
      "_id": {
        "$toString": "$_id"
      },
      "Name": 1,
      "ContentQueueID": 1,
      "ContentQueueName": "$result1.QueueInstance.Name"
}}]