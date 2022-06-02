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
* `executionStats.totalKeysExamined` amount of examined indexed keys (number of index entries scanned).

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

