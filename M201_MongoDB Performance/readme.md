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
  - [Partial indexes](#partial-indexes)
    - [Sparse indexes](#sparse-indexes)
  - [Text indexes](#text-indexes)
  - [Collations](#collations)
    - [Enabling case insensitive collection](#enabling-case-insensitive-collection)
  - [Wildcard indexes](#wildcard-indexes)
  - [Wildcard projection indexes](#wildcard-projection-indexes)
    - [Wildcard indexes unpredictable query shapes](#wildcard-indexes-unpredictable-query-shapes)
    - [Attribute pattern](#attribute-pattern)
  - [Labs 02](#labs-02)
    - [Index for sorting and filtering](#index-for-sorting-and-filtering)
    - [Finding best index](#finding-best-index)
- [Chapter 03: index operations](#chapter-03-index-operations)
  - [Hybrid index build](#hybrid-index-build)
  - [Query plans](#query-plans)
    - [Query plans - quize](#query-plans---quize)
  - [Forcing indexes with hint](#forcing-indexes-with-hint)
  - [Resource allocation for indexes](#resource-allocation-for-indexes)
    - [Memory allocation for indexes](#memory-allocation-for-indexes)
    - [Edge cases for indexes](#edge-cases-for-indexes)
  - [Basic benchmarking (comparing with different databases)](#basic-benchmarking-comparing-with-different-databases)
    - [Low level benchmarking](#low-level-benchmarking)
    - [Database server benchmarking](#database-server-benchmarking)
    - [Distributed Systems benchmarking](#distributed-systems-benchmarking)
    - [Benchmarking anti-patters](#benchmarking-anti-patters)
    - [Benchmarking conditions](#benchmarking-conditions)
  - [Lab 03](#lab-03)
- [Chapter 04: CRUD optimizations](#chapter-04-crud-optimizations)
  - [Index optimizations](#index-optimizations)
  - [Covered queries](#covered-queries)
  - [Regex performance](#regex-performance)
  - [Aggregation performance](#aggregation-performance)
    - [Aggregation performance for "realtime" processing](#aggregation-performance-for-realtime-processing)
      - [Index usage for aggregations](#index-usage-for-aggregations)
  - [Lab 04](#lab-04)
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

>NOTE: covered query is a query which can retrieve all the requested data from the index itself without needing to actually go to the collection.

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

## Partial indexes

It is possible to index only subset of documents - it is helpful if index with all data would be to big to keep it in memory

* Insert a restaurant document
```
db.restaurants.insertOne({
   "name" : "Han Dynasty",
   "cuisine" : "Sichuan",
   "stars" : 4.4,
   "address" : {
      "street" : "90 3rd Ave",
      "city" : "New York",
      "state" : "NY",
      "zipcode" : "10003"
   }
});
```

* Create an explainable object and run the query - we can see COLLSCAN
```
var exp = db.restaurants.explain()
exp.find({'address.city': 'New York', cuisine: 'Sichuan'})
```

* Create a partial index
```
db.restaurants.createIndex(
  { "address.city": 1, cuisine: 1 },
  { partialFilterExpression: { 'stars': { $gte: 3.5 } } }
)
```

* Adding the stars predicate allows us to use the partial index  
```
exp.find({'address.city': 'New York', cuisine: 'Sichuan', stars: { $gt: 4.0 }})
```

>NOTE: in order to use a partial index, the query must be guaranteed to match a subset of the documents, specified by the filter expression.

This will do `COLLSCAN`:

```
exp.find({'address.city': 'New York', cuisine: 'Sichuan'})
```
* Other rules
  * You cannot specify both the `partialFilterExpression` and the spares options
  * '_id' indexes cannot be partial indexes
  * Shared key indexes cannot be partial indexes


### Sparse indexes

Spars indexes are special case of partial indexes, these 2 are equivalents:

With a sparse index, we only index documents where the field exists that we are indexing on, 
rather than creating index key with null value.

```
db.restaurants.createIndex(
  { stars: 1 },
  { spares: true }
)

db.restaurants.createdIndex(
  { stars: 1 },
  { partialFilterExpression: { 'stars': { $exists: true } } }
)
```

Partial indexes are more flexible then sparse indexes because we can use in condition different field then field used in index:

```
db.restaurants.createdIndex(
  { stars: 1 },
  { partialFilterExpression: { 'cuisine': { $exists: true } } }
)
```

## Text indexes

```
db.textExample.insertOne({ "statement": "MongoDB is the best" })
db.textExample.insertOne({ "statement": "MongoDB is the worst." })
```

One way to query sub-strings is to use regular expressions:

```
db.textExample.find({ statement: /is the/ })
```

will return both documents but it is slow because it does not use any index.

Text indexes are much faster.

* Create text index `db.textExample.createIndex({ statement: "text" })`

* Next we can query using the index `db.textExample.find({ $text: { $search: "MongoDB best" } })`

  Under the hood they are very similar to multi-key indexes: for every unique word server will create index key.

  For example for text `MongoDB Long Sleeve T-Shirt` would generate such index keys (by default text indexes are case insensitive so everything is lower case): mongo, long, sleeve, t, shirt.
  This is because Unicode considers both spaces and hyphens as text delimiters.

* Display each document with it's "textScore" `db.textExample.find({ $text: { $search : "MongoDB best" } }, { score: { $meta: "textScore" } })`

* Sort the documents by their textScore so that the most relevant documents return first
`db.textExample.find({ $text: { $search : "MongoDB best" } }, { score: { $meta: "textScore" } }).sort({ score: { $meta: "textScore" } })`

* Sometimes text indexes can be very huge, one way is to reduce them is  to use compound indexes, for example
```
db.products.createIndex({ category: 1, productname: "text" });
```

Next we can narrow the query by specifying category name:
```
db.products.find({
  category: "Clothing",
  $text: { $search: "t-shirt" }
})
```

## Collations

It allow users to specify language specific rules for string comparison, such as rules for letter, case, and accent marks.

Collation schema:
```js
{
  locale: <string>,
  caseLevel: <boolean>,
  caseFirst: <string>,
  strength: <int>,
  numericOrdering: <boolean>,
  alternate: <string>,
  maxVariable: <string>,
  backwards: <boolean>,
}
```

* Create a collection-level collation for Portuguese `db.createCollection( "foreign_text", {collation: {locale: "pt"}})`
* Insert an example document `db.foreign_text.insert({ "name": "Máximo", "text": "Bom dia minha gente!"})`
* Explain the following query (uses the Portuguese collation) `db.foreign_text.find({ _id: {$exists:1 } } ).explain()`
  In `winningPlan.inputStage.collation` we can see details about used collation.
* Specify an Italian collation for a find query `db.foreign_text.find({ _id: {$exists:1 } }).collation({locale: 'it'})`
* Specify a Spanish collation for an aggregation query `db.foreign_text.aggregate([ {$match: { _id: {$exists:1 }  }}], {collation: {locale: 'es'}})`
* Create an index with a collation that differs from the collection collation `db.foreign_text.createIndex( {name: 1},  {collation: {locale: 'it'}} )`
* Uses the collection collation (Portuguese) `db.foreign_text.find( {name: 'Máximo'}).explain()` - this will use `COLLSACN` because we do not have an index for Portuguese collation.
* Uses the index collation (Italian) `db.foreign_text.find( {name: 'Máximo'}).collation({locale: 'it'}).explain()` - this will use `IXSCAN` because earlier we created index that has Italian collation.

### Enabling case insensitive collection
```
db.createCollection("no_sensitivity", { collation: { locale: "en", strength: 1 } })
```
Strength offers primary level of comparison, ignoring case and diacritics.

* Insert few records

```
db.no_sensitivity.insert({name: 'aaaaa'})
db.no_sensitivity.insert({name: 'aAAaa'})
db.no_sensitivity.insert({name: 'AaAaa'})
```

* Next we can see that different sorts return the same results because it is executed in case insensitive manner.
```
db.no_sensitivity.find().sort({name: 1})
db.no_sensitivity.find().sort({name: -1})
```

## Wildcard indexes

>NOTE: MongoDB 4.2 introduces wildcard indexes for supporting queries against unknown or arbitrary fields.

Some workloads have unpredictable access patterns. For instance sensor data for IoT use cases or weather stations.
In such cases, each query may include a combination of an arbitrary large number of different fields. This can make it very difficult
to plan an effective indexing strategy. For these workloads we need a way to be able to index on multiple fields without the overhead of maintaining multiple indexes - here comes with help wildcard indexes.

```
db.data.createIndex({'$**': 1})
```

The wildcard index will generate one virtual single field index at query execution time. Next the planner will assess them using the standard query plan score.

## Wildcard projection indexes

We can narrow the index to only some sub-documents.

* Include fields in the index
```
db.data.createIndex( {"sub_field.$**": 1}, { "wildcardProjection": { "included_field": 1 } } )
```
* Exclude fields in the index
```
db.data.createIndex( {"sub_field.$**": 1}, { "wildcardProjection": { "included_field": 0 } } )
```

* We can include, exclude also more nested sub-documents
```
db.data.createIndex( { "waveMeasurement.waves.$**": 1 } )
```

* **Wildcard indexes can cover queries if the query is on a single field only!** If the user is querying on a single field and uses
a projection which returns only that field, then the wildcard index can cover the query. For example:
```
db.data.find({
  { "waveMeasurement.waves.height": { $gt: 0.5 } },
  { _id: 0, "waveMeasurement.waves.height": 1 },
}).explain()
```

### Wildcard indexes unpredictable query shapes

![016_arbitrary-query-shape.png](./images/016_arbitrary-query-shape.png)

### Attribute pattern

Attribute pattern is a data modeling strategy that we can use to index and query across an arbitrary number of attributes.

![017_attribute-pattern.png](./images/017_attribute-pattern.png)

![018_attribute-pattern.png](./images/018_attribute-pattern.png)

![019_attribute-pattern.png](./images/019_attribute-pattern.png)

## Labs 02

### Index for sorting and filtering

```
db.personsCatalog.insertOne({
   "first_name" : "Han Dynasty",
   "ssn": "123456789",
   "address" : {
      "city" : "New York",
      "state" : "NY"
   }
});
db.personsCatalog.createIndex({ "first_name": 1, "address.state": -1, "address.city": -1, "ssn": 1 })
```

* Index will be used for both filtering and sorting
  ```
  db.personsCatalog.find({"first_name": "Jessica", "address.state": { $lt: "S" } }).sort( { "address.state": 1 } ).explain()
  db.personsCatalog.find({"address.state": "South Dakota", "first_name": "Jessica" }).sort( { "address.city": -1 } ).explain()
  db.personsCatalog.find({"first_name": "Jessica"}).sort( { "address.state": 1, "address.city": 1 } ).explain()
  ```

* Index will be used only for filtering

  **To use index for sorting on compound indexes all fields have to use the same order as index has or all fields have to use opposite order to order of the index.** Here in index we have `state: -1, city: -1` so to use this index we would have to sort -1, -1 or 1, 1.

  ```
  db.personsCatalog.find({"first_name": "Jessica"}).sort( { "address.state": 1, "address.city": -1 } ).explain()
  ```

  Here sorting will be in memory (index not used) because the query has a gap in used fields (missing `address.state`) so it is not "continues" operation.
  ```
  db.personsCatalog.find({"first_name": { $gt: "J" } }).sort( { "address.city": -1 } ).explain()
  ```

* Index will not be used at all

  ```
  db.personsCatalog.find({"address.city": "West Cindy"}).sort( { "address.city": -1 } ).explain()  
  ```

### Finding best index

All queries:

```
> db.people.find({
    "address.state": "Nebraska",
    "last_name": /^G/,
    "job": "Police officer"
  })

> db.people.find({
    "job": /^P/,
    "first_name": /^C/,
    "address.state": "Indiana"
  }).sort({ "last_name": 1 })

> db.people.find({
  "address.state": "Connecticut",
  "birthday": {
    "$gte": ISODate("2010-01-01T00:00:00.000Z"),
    "$lt": ISODate("2011-01-01T00:00:00.000Z")
  }
})
```

Possible indexes:

* { "address.state": 1, "last_name": 1, "job": 1 } - this is the best index for the above 3 queries
* { "job": 1, "address.state": 1, "first_name": 1 }
* { "job": 1, "address.state": 1 }
* { "address.state": 1, "job": 1, "first_name": 1 }
* { "job": 1, "address.state": 1, "last_name": 1 }
* { "address.state": 1, "job": 1 }

# Chapter 03: index operations

## Hybrid index build

In previous version of MongoDB there were 2 ways of building indexes:

* `Foreground` index build `db.movies.createIndex( { title: 1 } )`
  It was the most performant but had the unfortunate side effect of locking the entire database (or collection???) for the duration
  of the index build. This meant that could neither read from or write to the database.

* There is also way to run index creation using `background` flag, then it is not blocking because it uses incremental approach
  (periodical locks) but it is  not as performant as foreground index: `db.collection.createIndex( {title: 1}, { background: true } )`.
  If the index is larger than the available RAM, the incremental approach can take much longer than a foreground index build.
  Other downside of background indexes is that the index structure resulting from this type of index build is less efficient than foreground indexes, resulting in less optimal index traversal.

Disadvantages of both approaches have been addressed in new approach using `hybrid index build feature`. From 4.2 onwards, there will be no need for a background or a foreground index build - only one hybrid mechanism that takes the best of both worlds.

The new hybrid index build has both the performance of a foreground index build and the non-locking properties of a background index build, meaning that all database operations can proceed uninhibited. This is now the only way to build an index on MongoDB.

## Query plans

Sample stages: IXSCAN -> FETCH (storage engine converts the record IDs into documents) -> SORT (next documents are passed to SORT stage where an in-memory sort will ne performed on them).

* First the server will look at all available indexes on the collection. From there, it will identify which indexes are viable to satisfy  the query. We call these `candidate indexes`.

* From the `candidate indexes`, the query optimizer can generate candidate plans. MongoDB has what is called an `empirical query planner`,
  which means that there is going to be a trial period, where each of the candidate plans is executed over a short period of time.

* The planner will then see which plan performed best (it can have different meaning, best can be plan that returned all the results first,or it might be, which returned a certain number of documents - there are many ways to describe `best`).

* MongoDB cached which plan should use for given query shape - to not run trial every time.
  
### Query plans - quize

* MongoDB's query optimizer is statistically based, where collection heuristics are used to determine which plan wins.
  NO - MongoDB has an empirical query optimizer where query plans are ran against each other during a trial period.

* Query plans are cached so that plans do not need to be generated and compared against each other every time a query is executed.
  YES - that is correct.

* When query plans are generated, for a given query, every index generates at least one query plan.
  NO - only a subset of the indexes are considered as candidates for planning.

* If an index can't be used, then there is no query plan for that query.
  NO -  if there aren't any viable indexes for a given query, then a COLLSCAN stage will be the main stage of the query plan.

## Forcing indexes with hint

We can hint be providing field names or index name.
```
db.people.find({ name: "John Doe", zipcode: { $gt: "63000" } }).hint( {name: 1, zipcode: 1} )
db.people.find({ name: "John Doe", zipcode: { $gt: "63000" } }).hint( "name_1_zipcode_1" )
```

Sometimes it is better to remove not needed index instead of using `hint`.

## Resource allocation for indexes

It is quite important to determine:
* index size
* resource allocation (disk, memory)
  * disk usually is not an issue at all (in rare cases if we do not have a space for index file then the index will not be created at all)
* edge cases

We can run for the whole DB:
```
Atlas atlas-otfvmj-shard-0 [primary] sandbox> db.stats()
{
  db: 'sandbox',
  collections: 8,
  views: 0,
  objects: Long("1050487"),
  avgObjSize: 191.3443859847861,
  dataSize: Long("201004790"),
  storageSize: Long("105394176"),
  totalFreeStorageSize: Long("0"),
  numExtents: Long("0"),
  indexes: 16,
  indexSize: Long("42258432"),
  indexFreeStorageSize: Long("0"),
  fileSize: Long("0"),
  nsSizeMB: 0,
  ok: 1
}
```

or we can run per collection:

```
tlas atlas-otfvmj-shard-0 [primary] sandbox> db.people.stats()
{
  ns: 'sandbox.people',
  size: 19574079,
  count: 50474,
  avgObjSize: 387,
  storageSize: 11235328,
  freeStorageSize: 1933312,
  capped: false,
  nindexes: 2,
  indexBuilds: [],
  totalIndexSize: 2600960,
  totalSize: 13836288,
  indexSizes: { _id_: 1798144, ssn_1: 802816 },
  scaleFactor: 1,
  ok: 1,
  '$clusterTime': {
    clusterTime: Timestamp(3, 1654793118),
    signature: {
      hash: Binary(Buffer.from("164e0366a6c1bbde3b1c308c98cb96d6f7307fdb", "hex"), 0),
      keyId: Long("7071029582547124225")
    }
  },
  operationTime: Timestamp(3, 1654793118)
}
```

### Memory allocation for indexes

**Our deployments should be sized in order to accommodate our indexes in RAM.** We should have enough memory to accommodate all our indexes. In other case disk will be used to traverse indexes. Some pages can be allocated in RAM and other pages will be on disk.

* We can specify cache size in this way:  `mongod --dbpath data --wiredTigerCacheSzieGB 1`.
* Checking how much of index is in memory, use stats.
  * First sore result in `stats` variable because it contains a lot of entries `var stats = db.people.stats({ indexDetails: true })`
  * Next we can read interesting parts.

    By checking `'bytes currently in the cache'` and compare it with total index size `db.people.stats().indexSizes.ssn_1`.

    ```
    Atlas atlas-otfvmj-shard-0 [primary] sandbox> stats.indexDetails.ssn_1.cache
    {
      'bytes currently in the cache': 4042,
      'bytes dirty in the cache cumulative': 0,
      'bytes read into cache': 951,
      'bytes written from cache': 0,
      'checkpoint blocked page eviction': 0,
      'checkpoint of history store file blocked non-history store page eviction': 0,
      'data source pages selected for eviction unable to be evicted': 0,
      'eviction gave up due to detecting an out of order on disk value behind the last update on the chain': 0,
      'eviction gave up due to detecting an out of order tombstone ahead of the selected on disk update': 0,
      'eviction gave up due to detecting an out of order tombstone ahead of the selected on disk update after validating the update chain': 0,
      'eviction gave up due to detecting out of order timestamps on the update chain after the selected on disk update': 0,
      'eviction walk passes of a file': 0,
      'eviction walk target pages histogram - 0-9': 0,
      'eviction walk target pages histogram - 10-31': 0,
      'eviction walk target pages histogram - 128 and higher': 0,
      'eviction walk target pages histogram - 32-63': 0,
      'eviction walk target pages histogram - 64-128': 0,
      'eviction walk target pages reduced due to history store cache pressure': 0,
      'eviction walks abandoned': 0,
      'eviction walks gave up because they restarted their walk twice': 0,
      'eviction walks gave up because they saw too many pages and found no candidates': 0,
      'eviction walks gave up because they saw too many pages and found too few candidates': 0,
      'eviction walks reached end of tree': 0,
      'eviction walks restarted': 0,
      'eviction walks started from root of tree': 0,
      'eviction walks started from saved location in tree': 0,
      'hazard pointer blocked page eviction': 0,
      'history store table insert calls': 0,
      'history store table insert calls that returned restart': 0,
      'history store table out-of-order resolved updates that lose their durable timestamp': 0,
      'history store table out-of-order updates that were fixed up by reinserting with the fixed timestamp': 0,
      'history store table reads': 0,
      'history store table reads missed': 0,
      'history store table reads requiring squashed modifies': 0,
      'history store table truncation by rollback to stable to remove an unstable update': 0,
      'history store table truncation by rollback to stable to remove an update': 0,
      'history store table truncation to remove an update': 0,
      'history store table truncation to remove range of updates due to key being removed from the data page during reconciliation': 0,
      'history store table truncation to remove range of updates due to out-of-order timestamp update on data page': 0,
      'history store table writes requiring squashed modifies': 0,
      'in-memory page passed criteria to be split': 0,
      'in-memory page splits': 0,
      'internal pages evicted': 0,
      'internal pages split during eviction': 0,
      'leaf pages split during eviction': 0,
      'modified pages evicted': 0,
      'overflow pages read into cache': 0,
      'page split during eviction deepened the tree': 0,
      'page written requiring history store records': 0,
      'pages read into cache': 1,
      'pages read into cache after truncate': 0,
      'pages read into cache after truncate in prepare state': 0,
      'pages requested from the cache': 0,
      'pages seen by eviction walk': 0,
      'pages written from cache': 0,
      'pages written requiring in-memory restoration': 0,
      'the number of times full update inserted to history store': 0,
      'the number of times reverse modify inserted to history store': 0,
      'tracked dirty bytes in the cache': 0,
      'unmodified pages evicted': 0
    }    
    ```

### Edge cases for indexes

* Occasional reports (executed much less frequently than operational workload)

  In such cases indexes do not have to be necessarily in RAM and such operations can be executed using secondary nodes and not primary.
  So those indexes are created only on designated nodes!


* Right-end-side index increments (data that grow monotonically), like counters, dates, and incremental IDs. Typically in IoT.
  
  In such cases there is a risk the an index will be unbalanced.

  ![020_unbalanced-tree.png](./images/020_unbalanced-tree.png)

  You only grow positively. That means that the tree will always grow in the right-hand side. If we add to that fact that we only 
  need to query on the most recent data, then the amount of index that actually need to be in RAM, is always going to be the 
  right-end side of your index. So we have to care about how much data from the recently added data - how are we going to be needing
  access all the time?

  ![021_edge-case.png](./images/021_edge-case.png)

  ## Basic benchmarking (comparing with different databases)

  ### Low level benchmarking
  
  * File I/O performance
  * Scheduler performance
  * Memory allocation and transfer speed
  * Thread performance
  * Database server performance
  * Transaction isolation
  * and more...

Can be used for example https://github.com/akopytov/sysbench

### Database server benchmarking

* Data set load
* Writes per second
* Reads per second
* Balanced workloads
* Read/write ratio

Can be used for example https://github.com/brianfrankcooper/YCSB

Note that most of these tools have been designed to test relational systems so some additional configuration might be needed.

### Distributed Systems benchmarking

* Linearization
* Serialization
* Fault tolerance

### Benchmarking anti-patters

* Database swap replace (like relational vs no-sql)
* Using mongo shell for write and read requests
* Using mongoimport to test write response
* Local laptop to run tests
* Using default MongoDB parameters

### Benchmarking conditions

* Hardware
* Clients
* Load

## Lab 03

```
> var exp = db.restaurants.explain("executionStats")
> exp.find({ "address.state": "NY", stars: { $gt: 3, $lt: 4 } }).sort({ name: 1 }).hint(REDACTED)
```

```
{
  "queryPlanner": {
    "plannerVersion": 1,
    "namespace": "m201.restaurants",
    "indexFilterSet": false,
    "parsedQuery": "REDACTED",
    "winningPlan": {
      "stage": "SORT",
      "sortPattern": {
        "name": 1
      },
      "inputStage": {
        "stage": "SORT_KEY_GENERATOR",
        "inputStage": {
          "stage": "FETCH",
          "inputStage": {
            "stage": "IXSCAN",
            "keyPattern": "REDACTED",
            "indexName": "REDACTED",
            "isMultiKey": false,
            "isUnique": false,
            "isSparse": false,
            "isPartial": false,
            "indexVersion": 1,
            "direction": "forward",
            "indexBounds": "REDACTED"
          }
        }
      }
    },
    "rejectedPlans": [ ]
  },
  "executionStats": {
    "executionSuccess": true,
    "nReturned": 3335,
    "executionTimeMillis": 20,
    "totalKeysExamined": 3335,
    "totalDocsExamined": 3335,
    "executionStages": "REDACTED"
  },
  "serverInfo": "REDACTED",
  "ok": 1
}
```

Given the redacted explain output above, select the index that was passed to hint:

* `{ "address.state": 1, "name": 1, "stars": 1 }` - No, if this index was used, then there would be no SORT stage.
* `{ "address.state": 1, "stars": 1, "name": 1 }` - Yes, this query wouldn't need to examine any extra index keys, so since nReturned and totalKeysExamined are both 3,335 we know this index was used.
* `{ "address.state": 1, "name": 1 }` - No, if this index was used, then there would be no SORT stage.
* `{ "address.state": 1 }` - No, if this index was used, then we would expect that we'd have to examine some unnecessary documents and index keys. Since there are 50 states in the US, and we have 1,000,000 documents we'd expect to examine about 20,000 documents, not the 3,335 we see in the output.

# Chapter 04: CRUD optimizations

Make sure that `restaurants` collection has only index on `_id` field (there are 1 000 000 docs in this collection).

## Index optimizations

Compound index fields order should reflect their selectivity - first columns with the best selectivity.

```
var exp = db.restaurants.explain("executionStats")
```

* Not optimal index
  ```
  db.restaurants.createIndex({ "address.zipcode": 1, "cuisine": 1, "stars": 1 })
  exp.find( { "address.zipcode": { $gt: '50000' }, cuisine: 'Sushi' } ).sort({ stars: -1 })
  ```
  We can see stats
  ```js
  nReturned: 11611,
  executionTimeMillis: 576,
  totalKeysExamined: 95988,
  totalDocsExamined: 11611,
  ```

* Better index

  Because `cuisine` has better selectivity. Index that starts from this field will be more optimal for the same query
  ```
  db.restaurants.createIndex({ "cuisine": 1, "address.zipcode": 1, "stars": 1 })
  exp.find( { "address.zipcode": { $gt: '50000' }, cuisine: 'Sushi' } ).sort({ stars: -1 })
  ```
  We can see that now execution stats are better
  ```js
  executionSuccess: true,
  nReturned: 11611,
  executionTimeMillis: 86,
  totalKeysExamined: 11611,
  totalDocsExamined: 11611,
  ```
  but in `winningPlan` we can still see that we are doing sorting in memory and do not use index for this.

  **We can only use an index for both filtering and sorting if the keys in our query predicate are equality conditions.
  First usage different condition causes that sorting will not use index!** Because zip code is a range query, we are not able to use that index for sorting.

* Even better index

  ```
  db.restaurants.createIndex({ "cuisine": 1, "stars": 1, "address.zipcode": 1 })
  exp.find( { "address.zipcode": { $gt: '50000' }, cuisine: 'Sushi' } ).sort({ stars: -1 })
  ```
  Now we can see that sorting and filtering is done using index
  ```js
  winningPlan.stage: 'FETCH',
  winningPlan.inputStage.stage: 'IXSCAN',
  ...
  executionSuccess: true,
  nReturned: 11611,
  executionTimeMillis: 49,
  totalKeysExamined: 11663,
  totalDocsExamined: 11611,  
  ```

  Optimal order of operations (usually works the best for most cases):

  ![022_esr.png](./images/022_esr.png)

  ## Covered queries

  Covered queries are fully covered by index keys, there is no need to go to documents/collection.

  ```
  var exp = db.restaurants.explain("executionStats")
  db.restaurants.createIndex({name: 1, cuisine: 1, stars: 1})
  exp.find({name: { $gt: 'L' }, cuisine: 'Sushi', stars: { $gte: 4.0 } }, { _id: 0, name: 1, cuisine: 1, stars: 1 })
  ```
  In execution plan we can see that there is no `FETCH` stage:
  ```js  
  winningPlan.stage: 'PROJECTION_COVERED',
  winningPlan.inputStage.stage: 'IXSCAN',
  ...
  executionSuccess: true,
  nReturned: 2870,
  executionTimeMillis: 14,
  totalKeysExamined: 2988,
  totalDocsExamined: 0,
  ```

  This query also uses INDEX but we have to do FETCH because when all fields are 0 in projection then will be returned all other fields 
  and these fields are not available in the index.
  ```
  exp.find({name: { $gt: 'L' }, cuisine: 'Sushi', stars: { $gte: 4.0 } }, { _id: 0, address: 0 })
  ```
  ```js
  winningPlan.stage: 'PROJECTION_DEFAULT',
  winningPlan.inputStage.stage: 'FETCH',
  winningPlan.inputStage.inputStage.stage: 'IXSCAN',
  ...
  executionSuccess: true,
  nReturned: 2870,
  executionTimeMillis: 16,
  totalKeysExamined: 2988,
  totalDocsExamined: 2870,
  ```

  ## Regex performance

  Usually regex does not have good performance. Regex will use index only if we will use carrot operator which means **starts with**:
  ```
  db.users.find( { username: /^kirby/ } )
  ```

  ## Aggregation performance

  Two types of aggregations:

  * "Realtime" processing
    * provide data for applications
    * query performance is more important
  * Batch processing
    * provide data for analytics
    * query performance is less important

```
db.orders.aggregate([...], { explain: true })
```

### Aggregation performance for "realtime" processing

#### Index usage for aggregations

Once the server encounters a stage that is not able to use indexes, all of the following stages will not longer be able to use indexes either. The query optimizer tries its best to detect when a stage can be moved forward so that indexes can be utilized.
Transforming data in a pipeline stage prevents us from using indexes in the stages that follow.

```
db.orders.createIndex({ cust_id: 1 })
```

```
db.orders.aggregate([
  { $march: { cust_id: "287" } }, // index will be used
  ...
])
```

```
db.orders.aggregate([
  { $march: { cust_id: { $lt: 50 } } }, // index will be used
  { $sort: { cust_id: 1 } }, index will be used
  ...
])
```

* top-key sorting algorithm

  If you are doing a limit and doing a sort, you want to make sure that they are near each other and at the front of the pipeline.
  When this happens, the server is able to do a top-k sort. This is when the server is able to only allocate memory for the final number of documents, in this case 10. This can happen even without indexes. This is one of the most highly performant non-index situations
  that you can be in.

  ```
  db.orders.aggregate([
    { $march: { cust_id: { $lt: 50 } } }, // index will be used
    { $limit: 10 },
    { $sort: { total: 1 } },
    ...
  ])
  ```

* memory constraints
  * results are subject to 16 MB document limit. Aggregation generally outputs a single document and that single document will be       
  susceptible to this limit
  * 100MB of RAM per stage (to mitigate this limit use indexes)
    * `db.orders.aggregate([...], { allowDiskUse: true } )` but it will be very slow (not good for real time processing)!. It does support `$graphLookup`.


## Lab 04

Drop all indexes (`_id` index will stay).
```
db.restaurants.dropIndexes()
```
Then, run the following query and you will receive an error:
```
db.restaurants.aggregate([
      { $match: { stars: { $gt: 2 } } },
      { $sort: { stars: 1 } },
      { $group: { _id: "$cuisine", count: { $sum: 1 } } }
  ])
MongoServerError: PlanExecutor error during aggregation :: caused by :: Sort exceeded memory limit of 33554432 bytes, but did not opt in to external sorting.
```

To solve this problem we have to create the following index
```
db.restaurants.createIndex( { stars: 1 } )
db.restaurants.aggregate([
      { $match: { stars: { $gt: 2 } } },
      { $sort: { stars: 1 } },
      { $group: { _id: "$cuisine", count: { $sum: 1 } } }
  ], { explain: true })
```