- [Chapter 01: Introduction](#chapter-01-introduction)
  - [Uploading sample data](#uploading-sample-data)
- [Chapter 02: MongoDB Indexes](#chapter-02-mongodb-indexes)
  - [Two ways of storing database files](#two-ways-of-storing-database-files)
  - [Journal](#journal)
  - [Single Field Indexes](#single-field-indexes)
    - [Read data without index](#read-data-without-index)
    - [Create index and test it](#create-index-and-test-it)
      - [Index on sub-document](#index-on-sub-document)
    - [Range value, in value, multiple fields with index](#range-value-in-value-multiple-fields-with-index)
  - [Understanding `explain` command](#understanding-explain-command)
    - [Sorting](#sorting)
  - [Sorting with indexes - deep dive](#sorting-with-indexes---deep-dive)
    - [Sort by ssn](#sort-by-ssn)
    - [Sort by field with no index](#sort-by-field-with-no-index)
    - [Sort ssn descending](#sort-ssn-descending)
  - [Querying on Compound Indexes](#querying-on-compound-indexes)
    - [Index prefixes for filtering](#index-prefixes-for-filtering)
  - [Sorting using compound indexes](#sorting-using-compound-indexes)
      - [Sorting using compound indexes - ascending, descending](#sorting-using-compound-indexes---ascending-descending)
  - [Multikey Indexes](#multikey-indexes)
# Chapter 01: Introduction

* Memory
  * Aggregation
  * Index Traversing
  * Write Operations
  * Query Engine
  * Connections (1MB per connection)

* CPU
  * Storage Engine
  * Concurrency Model (locking causes that we might not be able consume all available CPU - because of waiting time)
  * Page Compression
  * Data Calculation
  * Aggregation Framework Operations
  * Map Reduce

* Disk

Recommended RAID for MongoDB is RAID 10 or RAID 1-0.

## Uploading sample data

Sample data are available in introduction chapter of this course.
I pasted downloaded files into `D:\Programs\mongodb-database-tools-windows-x86_64-100.5.0\mongodb-database-tools-windows-x86_64-100.5.0\bin` to have short names in parameters. Before uploading make sure that your current IP address is on the whitelist.

* Upload people

```
PS D:\Programs\mongodb-database-tools-windows-x86_64-100.5.0\mongodb-database-tools-windows-x86_64-100.5.0\bin> mongoimport --uri "mongodb+srv://super-kicaj:kicaj@sandbox.kxcwk.mongodb.net" --file people.json --collection people --db sandbox --drop
2022-05-31T10:36:55.624+0200    connected to: mongodb+srv://[**REDACTED**]@sandbox.kxcwk.mongodb.net
2022-05-31T10:36:55.654+0200    dropping: sandbox.people
2022-05-31T10:36:58.632+0200    [#.......................] sandbox.people       1.75MB/21.8MB (8.0%)
2022-05-31T10:37:01.633+0200    [###.....................] sandbox.people       3.04MB/21.8MB (14.0%)
2022-05-31T10:37:04.628+0200    [####....................] sandbox.people       3.90MB/21.8MB (17.9%)
2022-05-31T10:37:07.633+0200    [#####...................] sandbox.people       5.19MB/21.8MB (23.8%)
2022-05-31T10:37:10.636+0200    [#######.................] sandbox.people       6.48MB/21.8MB (29.8%)
2022-05-31T10:37:13.637+0200    [########................] sandbox.people       7.77MB/21.8MB (35.7%)
2022-05-31T10:37:16.626+0200    [#########...............] sandbox.people       9.06MB/21.8MB (41.6%)
2022-05-31T10:37:19.626+0200    [###########.............] sandbox.people       10.4MB/21.8MB (47.6%)
2022-05-31T10:37:22.638+0200    [############............] sandbox.people       11.7MB/21.8MB (53.5%)
2022-05-31T10:37:25.627+0200    [##############..........] sandbox.people       12.9MB/21.8MB (59.5%)
2022-05-31T10:37:28.631+0200    [###############.........] sandbox.people       14.2MB/21.8MB (65.4%)
2022-05-31T10:37:31.638+0200    [#################.......] sandbox.people       15.5MB/21.8MB (71.4%)
2022-05-31T10:37:34.631+0200    [##################......] sandbox.people       16.8MB/21.8MB (77.3%)
2022-05-31T10:37:37.635+0200    [###################.....] sandbox.people       18.1MB/21.8MB (83.3%)
2022-05-31T10:37:40.629+0200    [#####################...] sandbox.people       19.4MB/21.8MB (89.2%)
2022-05-31T10:37:43.633+0200    [######################..] sandbox.people       20.7MB/21.8MB (95.2%)
2022-05-31T10:37:46.632+0200    [########################] sandbox.people       21.8MB/21.8MB (100.0%)
2022-05-31T10:37:46.775+0200    [########################] sandbox.people       21.8MB/21.8MB (100.0%)
2022-05-31T10:37:46.776+0200    50474 document(s) imported successfully. 0 document(s) failed to import.
```

* Upload restaurants

```
PS D:\Programs\mongodb-database-tools-windows-x86_64-100.5.0\mongodb-database-tools-windows-x86_64-100.5.0\bin> mongoimport --uri "mongodb+srv://super-kicaj:kicaj@sandbox.kxcwk.mongodb.net" --file restaurants.json --collection restaurants --db sandbox --drop
2022-05-31T10:40:45.424+0200    connected to: mongodb+srv://[**REDACTED**]@sandbox.kxcwk.mongodb.net
2022-05-31T10:40:45.468+0200    dropping: sandbox.restaurants
2022-05-31T10:40:48.435+0200    [........................] sandbox.restaurants  741KB/144MB (0.5%)
2022-05-31T10:40:51.430+0200    [........................] sandbox.restaurants  1.01MB/144MB (0.7%)
2022-05-31T10:40:54.438+0200    [........................] sandbox.restaurants  1.44MB/144MB (1.0%)
2022-05-31T10:40:57.427+0200    [........................] sandbox.restaurants  1.88MB/144MB (1.3%)
2022-05-31T10:41:00.430+0200    [........................] sandbox.restaurants  2.31MB/144MB (1.6%)
2022-05-31T10:41:03.440+0200    [........................] sandbox.restaurants  2.74MB/144MB (1.9%)
2022-05-31T10:41:06.440+0200    [........................] sandbox.restaurants  3.17MB/144MB (2.2%)
2022-05-31T10:41:09.438+0200    [........................] sandbox.restaurants  3.60MB/144MB (2.5%)
....

```

* Run sample query in mongosh
```
PS D:\Programs\mongosh-1.0.1-win32-x64\bin> .\mongosh "mongodb+srv://super-kicaj:kicaj@sandbox.kxcwk.mongodb.net"
Current Mongosh Log ID: 6295d5e54fccd474275fbbb4
Connecting to:          mongodb+srv://<credentials>@sandbox.kxcwk.mongodb.net/
Using MongoDB:          5.0.8
Using Mongosh:          1.0.1

For mongosh info see: https://docs.mongodb.com/mongodb-shell/

Atlas atlas-otfvmj-shard-0 [primary] test> use sandbox
switched to db sandbox
Atlas atlas-otfvmj-shard-0 [primary] sandbox> db.people.count({ "email" : {"$exists": 1} })
50474
Atlas atlas-otfvmj-shard-0 [primary] sandbox>
```

* To connect using MongoDB Compass paste this connection string `mongodb+srv://super-kicaj:kicaj@sandbox.kxcwk.mongodb.net`



# Chapter 02: MongoDB Indexes

## Two ways of storing database files

* [directoryPerDB](https://www.mongodb.com/docs/manual/reference/configuration-options/#mongodb-setting-storage.directoryPerDB)  
  * wiredtiger directory for indexes (`--wiredTigerDirectoryForIndexes`, it is param of `mongod`)   
    Then will be used 2 directories: `collection` and `index` inside DB directory. It allows mounting 2 different disk and parallelization in writing.
    ![001_paraller_write_collection_and_indexes](./images/001_paraller_write_collection_and_indexes.png)
* all databases in the same directory

## Journal
https://www.mongodb.com/docs/manual/core/journaling/
https://www.youtube.com/watch?v=nDDWOxwot7o

[Oplog vs Journal](https://www.quora.com/Whats-the-difference-between-MongoDBs-oplog-and-journal-When-is-data-written-to-them)

"Oplog has information of operations (as whole) done in bson format (like insert and then json record). These oplog entries are replicated to other nodes of replica set.

Journal have byte-level information of changes at (disk) data-blocks after last written checkpoint. With that information, the system can make recovery after a dirty shutdown."

![002_jounral.png](./images/002_jounral.png)

To minimize the performance impact of journal, the journal flushes our performed using group commits in a compressed format. All writes to the journal are atomic.

To sync data with the journal before acknowledging write operation use `j:true` option but it has impact on performance.

```
db.collection.insert({...}, {writeConcern: {w: 1, j:true}})
```
## Single Field Indexes

Connect to `mongosh`:
```
PS D:\Programs\mongosh-1.0.1-win32-x64\bin> .\mongosh "mongodb+srv://super-kicaj:kicaj@sandbox.kxcwk.mongodb.net"
```

### Read data without index

To collect stats use `explain` command.

```
db.people.find({ "ssn" : "720-38-5636" }).explain("executionStats")
```
It will return many information, for now we will focus on few:

* `queryPlanner.winningPlan`
  * EOF means that probably we are in wrong namespace or the table does not exist at all.
  * COLLSCAN means that we do not use index and we are checking every document
* `executionStats.totalDocsExamined` returns amount of documents that have been checked.
* `executionStats.nReturned` amount of returned document.
* `executionStats.totalKeysExamined` amount of examined indexed keys (number of index entries scanned). If it is greater than 0 we know that some index has been used.

### Create index and test it

Create ascending index (1):
```
Atlas atlas-otfvmj-shard-0 [primary] sandbox> db.people.createIndex( { ssn : 1 } )
ssn_1
Atlas atlas-otfvmj-shard-0 [primary] sandbox>
```

If some documents do not have SSN field then that key entry is going to have a null value.

Create an explainable object for the people collection:
`exp = db.people.explain("executionStats")`.

```
Atlas atlas-otfvmj-shard-0 [primary] sandbox> exp = db.people.explain("executionStats")
Explainable(sandbox.people)
Atlas atlas-otfvmj-shard-0 [primary] sandbox> exp.find( { "ssn" : "720-38-5636" } )
```

* `queryPlanner.winningPlan` is `IXSCAN`
* `executionStats.totalDocsExamined` is 1
* `executionStats.nReturned` is 1
* `executionStats.totalKeysExamined` is 1

Next we can use the same `exp` to check how will look another sample query:
```
exp.find( { last_name : "Acevedo" } )
```

#### Index on sub-document

* Insert 2 documents with sub-document.
```
Atlas atlas-otfvmj-shard-0 [primary] sandbox> db.examples.insertOne( { _id : 0, subdoc : { indexedField: "value", otherField : "value" } } )
{ acknowledged: true, insertedId: 0 }
Atlas atlas-otfvmj-shard-0 [primary] sandbox> db.examples.insertOne( { _id : 1, subdoc : { indexedField : "wrongValue", otherFotherField : "value" } } )
{ acknowledged: true, insertedId: 1 }
```

* Next create index
```
Atlas atlas-otfvmj-shard-0 [primary] sandbox> db.examples.createIndex( { "subdoc.indexedField" : 1 } )
subdoc.indexedField_1
```

* Next check execution plan
```
Atlas atlas-otfvmj-shard-0 [primary] sandbox> db.examples.explain("executionStats").find( { "subdoc.indexedField" : "value" })
```

>NOTE: we should never index on the field that points to a sub-document. Because doing so, we would have to query on the entire sub-document. It is much better to use dot notation when querying because we can just query on the fields that we care about in our sub-documents. If you do need to index on more than one field, you can use compound index.

### Range value, in value, multiple fields with index

```
exp.find( { ssn : { $gte : "555-00-0000", $lt : "556-00-0000" } } )
```

* `queryPlanner.winningPlan` is `IXSCAN`
* `executionStats.totalDocsExamined` is 49
* `executionStats.nReturned` is 49
* `executionStats.totalKeysExamined` is 49

```
exp.find( { "ssn" : { $in : [ "001-29-9184", "177-45-0950", "265-67-9973" ] } } )
```

* `queryPlanner.winningPlan` is `IXSCAN`
* `executionStats.totalDocsExamined` is 3
* `executionStats.nReturned` is 3
* `executionStats.totalKeysExamined` is 6

`totalKeysExamined` equals `6` is due to the algorithm the system used, which can involve overshooting the values we are looking for.

```
exp.find( { "ssn" : { $in : [ "001-29-9184", "177-45-0950", "265-67-9973" ] }, last_name : { $gte : "H" } } )
```

* `queryPlanner.winningPlan` is `IXSCAN`
* and we have additional `filter` object that points executed filtering on documents returned by the idnex.

## Understanding `explain` command

Can help with answering these questions:

* Is your query using the index you expect?
* Is your query using an index to provide sort?
* Is your query using an index to provide projection?
* How selective is your index?
* Which part of your plan is the most expensive?


* `db.people.explain()`
* `db.people.explain("queryPlanner")` - does the same as a call without parameter, it **does not execute the query**
* `db.people.explain("executionStats")` - it will **execute** the query and next will return different stats about this execution 
`db.people.explain("allPlansExecution")` - will **execute** the query and will return all possible stats (the most verbose option)


>NOTE: In good execution plan `executionStats.nReturned` is the same as 
`executionStats.totalKeysExamined` and `executionStats.totalDocsExamined`.

If there are two indexes to consider execution plan will contain additional object `rejectedPlans`.

```
expRun = db.people.explain("executionStats")
db.people.createIndex({last_name:1})
expRun.find({"last_name":"Johnson", "address.state":"New York"})
db.people.createIndex({"address.state": 1, last_name: 1})
expRun.find({"last_name":"Johnson", "address.state":"New York"})
```

### Sorting

```
var res = db.people.find({"last_name":"Johnson", "address.state":"New York"}).sort({"birthday":1}).explain("executionStats")
res.executionStats.executionStages
```

This will return plan with the following order: first is executed IXSCAN, next we are doing FETCH, and after fetch we are doing sorting using SORT.
We can see that the execution starts with the most nested element of the plan.

The `stage: 'SORT'` tells us the index was not use for the sort and a sort had to done, so it had to be done in memory.

```js
{
  stage: 'SORT',
  nReturned: 7,
  executionTimeMillisEstimate: 0,
  works: 17,
  advanced: 7,
  needTime: 8,
  needYield: 0,
  saveState: 0,
  restoreState: 0,
  isEOF: 1,
  sortPattern: { birthday: 1 },
  memLimit: 33554432,
  type: 'simple',
  totalDataSizeSorted: 3062,
  usedDisk: false,
  inputStage: {
    stage: 'FETCH',
    nReturned: 7,
    executionTimeMillisEstimate: 0,
    works: 8,
    advanced: 7,
    needTime: 0,
    needYield: 0,
    saveState: 0,
    restoreState: 0,
    isEOF: 1,
    docsExamined: 7,
    alreadyHasObj: 0,
    inputStage: {
      stage: 'IXSCAN',
      nReturned: 7,
      executionTimeMillisEstimate: 0,
      works: 8,
      advanced: 7,
      needTime: 0,
      needYield: 0,
      saveState: 0,
      restoreState: 0,
      isEOF: 1,
      keyPattern: { 'address.state': 1, last_name: 1 },
      indexName: 'address.state_1_last_name_1',
      isMultiKey: false,
      multiKeyPaths: { 'address.state': [], last_name: [] },
      isUnique: false,
      isSparse: false,
      isPartial: false,
      indexVersion: 2,
      direction: 'forward',
      indexBounds: {
        'address.state': [ '["New York", "New York"]' ],
        last_name: [ '["Johnson", "Johnson"]' ]
      },
      keysExamined: 7,
      seeks: 1,
      dupsTested: 0,
      dupsDropped: 0
    }
  }
}
```

## Sorting with indexes - deep dive

* in memory (if more then 32MB is needed then sorting will be rejected)
* using index

It is important to note that the query planner considers indexes that can be helpful to either the query predicate or to the requested sort.

Reset people collection `mongoimport --uri "mongodb+srv://super-kicaj:kicaj@sandbox.kxcwk.mongodb.net" --file people.json --collection people --db sandbox --drop`.

and again create indexe
```
db.people.createIndex( { ssn : 1 } )
```
### Sort by ssn

Create execution plan:
```
var exp = db.people.explain('executionStats')
exp.find({}, {_id: 0, last_name: 1, fist_name: 1, ssn: 1}).sort({ssn: 1})
```

We can see these values:
```js
executionStats.totalKeysExamined: 50474,
executionStats.totalDocsExamined: 50474,
...
winningPlan.stage: 'PROJECTION_SIMPLE',
winningPlan.inputStage.stage: 'FETCH',
winningPlan.inputStage.inputStage.stage: 'IXSCAN',
```

We see so  many total keys and docs because the index was not used for filtering but for sorting.

### Sort by field with no index

If we sort on field that does not have index `exp.find({}, {_id: 0, last_name: 1, fist_name: 1, ssn: 1}).sort({first_name: 1})` then in the plan we will see:

```js
executionStats.totalKeysExamined: 0,
executionStats.totalDocsExamined: 50474,
...
winningPlan.stage: 'PROJECTION_SIMPLE',
winningPlan.inputStage.stage: 'SORT' //SORT means that sorting has been executed in memory
winningPlan.inputStage.inputStage.stage: 'COLLSCAN'
```

### Sort ssn descending

If we sort by `ssn descending` we can still use index because it supports **traversing backwards** `exp.find({}, {_id: 0, last_name: 1, fist_name: 1, ssn: 1}).sort({ssn: -1})`.


```js
executionStats.totalKeysExamined: 50474,
executionStats.totalDocsExamined: 50474,
...
winningPlan.stage: 'PROJECTION_SIMPLE',
winningPlan.inputStage.stage: 'FETCH',
winningPlan.inputStage.inputStage.stage: 'IXSCAN',
winningPlan.inputStage.inputStage.direction: 'backward',
```

>NOTE: when we are sorting with a single field index, we can always sort documents either ascending or descending.

## Querying on Compound Indexes

If you have 2 fields, you might somehow think that your indexes are two-dimensional in some way, but they are not. They are one-dimensional, so it is like and ordered list.

![003_more-fields-indexes.png](./images/003_more-fields-indexes.png)

In this exercise we will use local mongodb.

```
PS D:\Programs\mongodb-database-tools-windows-x86_64-100.5.0\mongodb-database-tools-windows-x86_64-100.5.0\bin> mongoimport --uri "mongodb://localhost:27017" --file people.json --collection people --db m201 --drop
2022-06-08T15:01:31.573+0200    connected to: mongodb://localhost:27017
2022-06-08T15:01:31.576+0200    dropping: m201.people
2022-06-08T15:01:32.846+0200    50474 document(s) imported successfully. 0 document(s) failed to import.
```

* Run query without indexes
  
![004_multiple-fields-no-indexes.png](./images/004_multiple-fields-no-indexes.png)

* Create single field index (last name) and run the query again

![004_single-field-index.png](./images/004_single-field-index.png)

![005_single-field-index-plan.png](./images/005_single-field-index-plan.png)

* Create single field index (first name) and run the query again

We can see that now we have 2 indexes (last name, fist name)

![006_second-single-field-index.png](./images/006_second-single-field-index.png)

If we run the plan we can see that it did not change, so it means that mongo did not "join" these 2 indexes in this query

![007_second-single-field-index-plan.png](./images/007_second-single-field-index-plan.png)


* Create compound index and run the query again

![008_compound-index-create.png](./images/008_compound-index-create.png)

We can see that now the plan looks much better - perfect ratio between Index Keys Examined and Documents Examined.

![009_compound-index-create-plan.png](./images/009_compound-index-create-plan.png)

* Using range in query

![010_compound-index-range-plan.png](./images/010_compound-index-range-plan.png)

### Index prefixes for filtering

For a query on multiple fields that overlap with the index, identify which
fields in the query can use the index. Basically we use only part of fields that create the whole index. 

Remove previously created indexes and create new compound index.

![011_compound-index-4-fields.png](./images/011_compound-index-4-fields.png)

`{"job": "Jewellery designer", "employer": "Baldwin-Nichols"}`
![012_compound-index-4-fields-query1.png](./images/012_compound-index-4-fields-query1.png)

`{"job": "Jewellery designer", "employer": "Baldwin-Nichols", "last_name": "Cook"}`
![013_compound-index-4-fields-query2.png](./images/013_compound-index-4-fields-query2.png)

This query is less performant because we had to check 5 not necessary records in the index. It happened because we do not use the index in "continuous" way.
`{"job": "Jewellery designer", "employer": "Baldwin-Nichols", "first_name": "Sara"}`
![014_compound-index-4-fields-query3.png](./images/014_compound-index-4-fields-query3.png)

This query is even worst `{"job": "Jewellery designer", "first_name": "Sara", "last_name": "Cook"}` because `employer` is not used for filtering.

![015_compound-index-4-fields-query4.png](./images/015_compound-index-4-fields-query4.png)

```js
"indexBounds": {
  "job": [
   "[\"Jewellery designer\", \"Jewellery designer\"]"
  ],
  "employer": [
   "[MinKey, MaxKey]"
  ],
  "last_name": [
   "[\"Cook\", \"Cook\"]"
  ],
  "first_name": [
   "[\"Sara\", \"Sara\"]"
  ]
 },
```

## Sorting using compound indexes

```
.\mongosh "mongodb://localhost:27071"
```

Check current index
```js
test> use m201                                                
switched to db m201                                           
m201> db.people.getIndexes()                                  
[                                                             
  { v: 2, key: { _id: 1 }, name: '_id_' },                    
  {                                                           
    v: 2,                                                     
    key: { job: 1, employer: 1, last_name: 1, first_name: 1 },
    name: 'job_employer_last_name_first_name',                
    background: false                                         
  }                                                           
]
m201> var exp = db.people.explain("executionStats")
Explainable(m201.people)
m201> exp.find({}).sort({ job: 1, employer: 1, last_name : 1, first_name : 1 })
```

We can see that we still do index scan to do sorting - **there is no SORT stage**.

```js
queryPlanner.winningPlan.stage: 'FETCH',
queryPlanner.winningPlan.inputStage.stage: 'IXSCAN',
queryPlanner.winningPlan.inputStage.indexBounds: {
          job: [ '[MinKey, MaxKey]' ],
          employer: [ '[MinKey, MaxKey]' ],
          last_name: [ '[MinKey, MaxKey]' ],
          first_name: [ '[MinKey, MaxKey]' ]
        }
```

This query will also use index for sorting `exp.find({}).sort({ job: 1, employer: 1 })`

But this query `exp.find({}).sort({ employer: 1, job: 1 })` will not use index for sorting because we sort in a way which is not used in the index, so we get:

```js
queryPlanner.winningPlan.stage: 'SORT',
queryPlanner.winningPlan.inputStage.stage: 'COLLSCAN',
```

This will still use the index for sorting:
`exp.find({ email:"jenniferfreeman@hotmail.com" }).sort({ job: 1 })`


This doesn't follow an index prefix, and can't use the index for sorting, only filtering: `exp.find({ job: 'Graphic designer' }).sort({ last_name: 1 })`:

```js
queryPlanner.winningPlan.stage: 'FETCH',
queryPlanner.winningPlan.inputStage.stage: 'SORT',
queryPlanner.winningPlan.inputStage.inputStage.stage: 'IXSCAN',
```
#### Sorting using compound indexes - ascending, descending

Uses the index for sorting: `exp.find().sort({job: -1, employer: -1})`, `exp.find().sort({job: 1, employer: 1})`

Sorting is done in-memory: `exp.find().sort({job: -1, employer: 1})`, `exp.find().sort({job: 1, employer: -1})`. 

Example with new collection:

```js
m201> db.coll.createIndex({ a: 1, b: -1, c: 1 })
a_1_b_-1_c_1
m201> var expColl = db.coll.explain("executionStats")
Explainable(m201.coll)
```

* `expColl.find().sort({ a: 1, b: -1, c: 1 })` will use index for sorting.
* `expColl.find().sort({ a: -1, b: 1, c: -1 })` will use index for sorting.
* `expColl.find().sort({ a: 1, b: 1, c: -1 })` will not use index at all for sorting.


**To use index for sorting on compound indexes all fields have to use the same order as index has or all fields have to use opposite order to order of the index.**

## Multikey Indexes

When we create an index on a field which is an array, this is what we call multikey index.
It is called this because for each entry in the array, the server will create a separate index key.

Multikey indexes are not supported on multiple fields which are arrays because we would create cartesian product - such index would be to huge to be able run queries fast on it.

Multikey indexes do not support covered queries.

We will use atlas mongodb: `.\mongosh "mongodb+srv://super-kicaj:kicaj@sandbox.kxcwk.mongodb.net"`.

* Insert sample record

```
db.products.insertOne({
  productName: "MongoDB Short Sleeve T-Shirt",
  categories: ["T-Shirts", "Clothing", "Apparel"],
  stock: { size: "L", color: "green", quantity: 100 }
});
```

* Create index `db.products.createIndex({ "stock.quantity": 1})`

* Check execution plan

```
var exp = db.products.explain()
exp.find({ "stock.quantity": 100 })
```

We can see that index has been used and `'isMultiKey': false` which makes sense because the index is not for array.

* Insert document that has stock field as an array

```
db.products.insertOne({
  productName: "MongoDB Long Sleeve T-Shirt",
  categories: ["T-Shirts", "Clothing", "Apparel"],
  stock: [
    { size: "S", color: "red", quantity: 25 },
    { size: "S", color: "blue", quantity: 10 },
    { size: "M", color: "blue", quantity: 50 }
  ]
});
```

* Re-run the same query again `exp.find({ "stock.quantity": 100 })`.
We can see that now `'isMultiKey': true` even if we read document that does not have stock as an array - it means that MongoDB only recognizes that an index is multikey when a document is inserted where that field is an array.

* If we try create compound index on two fields and both are array then it will fail

```
Atlas atlas-otfvmj-shard-0 [primary] sandbox> db.products.createIndex({ categories: 1, "stock.quantity": 1 })
MongoServerError: Index build failed: 37152ff4-a9a8-4cdb-b1be-c6f75bee1d50: Collection sandbox.products ( ff32f168-af83-4bae-9f19-3a36528c2437 ) :: caused by :: cannot index parallel arrays [stock] [categories]
```

* But compound indexes with only 1 array field are good `db.products.createIndex({ productName: 1, "stock.quantity": 1 })`.

* Next we can insert document where `productName` is an array but `stock` is not

```
db.products.insertOne({
  productName: [
    "MongoDB Short Sleeve T-Shirt",
    "MongoDB Short Sleeve Shirt"
  ],
  categories: ["T-Shirts", "Clothing", "Apparel"],
  stock: { size: "L", color: "green", quantity: 100 }
});
```

* But we cannot insert a document where `productName` and `stock` are arrays.

```
Atlas atlas-otfvmj-shard-0 [primary] sandbox> db.products.insertOne({
...   productName: [
...     "MongoDB Short Sleeve T-Shirt",
...     "MongoDB Short Sleeve Shirt"
...   ],
...   categories: ["T-Shirts", "Clothing", "Apparel"],
...   stock: [
...     { size: "S", color: "red", quantity: 25 },
...     { size: "S", color: "blue", quantity: 10 },
...     { size: "M", color: "blue", quantity: 50 }
...   ]
... });
MongoServerError: cannot index parallel arrays [stock] [productName]
```