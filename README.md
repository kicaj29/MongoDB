- [Basics](#basics)
  - [MongoDB is document database type.](#mongodb-is-document-database-type)
  - [CAP model for MongoDB](#cap-model-for-mongodb)
  - [SQL Terms vs Mongo DB Terms](#sql-terms-vs-mongo-db-terms)
  - [JSON vs BSON](#json-vs-bson)
  - [Replica Set](#replica-set)
  - [Causal consistency](#causal-consistency)
- [Setting up MongoDB Atlas](#setting-up-mongodb-atlas)
  - [Create a new project and next go to this project](#create-a-new-project-and-next-go-to-this-project)
  - [Create a free cluster](#create-a-free-cluster)
    - [Load sample data](#load-sample-data)
    - [Monitor create cluster](#monitor-create-cluster)
    - [View sample data](#view-sample-data)
    - [DB access](#db-access)
- [CRUD operations](#crud-operations)
  - [Create](#create)
    - [Create using MongoDB Compass](#create-using-mongodb-compass)
    - [Create using command line](#create-using-command-line)
      - [Create a new database using command line](#create-a-new-database-using-command-line)
      - [insertOne](#insertone)
        - [Reading table](#reading-table)
      - [insertMany](#insertmany)
  - [Reading data](#reading-data)
    - [Query and query operators](#query-and-query-operators)
    - [Query projections](#query-projections)
    - [Read concerns](#read-concerns)
    - [Reading using MongoDB compass](#reading-using-mongodb-compass)
    - [Reading using command line](#reading-using-command-line)
      - [select existing DB](#select-existing-db)
      - [list of collections](#list-of-collections)
      - [query all movies](#query-all-movies)
      - [returns next set of results](#returns-next-set-of-results)
      - [pretty](#pretty)
      - [projection (selecting which fields should be displayed):](#projection-selecting-which-fields-should-be-displayed)
      - [sorting](#sorting)
      - [specifying read concerns](#specifying-read-concerns)
  - [Write concerns](#write-concerns)
  - [Update](#update)
    - [Update in MongoDB Compass](#update-in-mongodb-compass)
    - [Update in command line](#update-in-command-line)
      - [Update single document](#update-single-document)
      - [Update multiple documents](#update-multiple-documents)
        - [Using `upsert`](#using-upsert)
      - [Specify write concern](#specify-write-concern)
    - [Replace command](#replace-command)
  - [Delete](#delete)
    - [Delete in MongoDB Compass](#delete-in-mongodb-compass)
    - [Delete in command line](#delete-in-command-line)
      - [Delete one](#delete-one)
      - [Delete many](#delete-many)
      - [Remove](#remove)
- ['Foreign key constraint' in MongoDB](#foreign-key-constraint-in-mongodb)

# Basics
## MongoDB is document database type.

Advantages of document database type:
* intuitive data model
* flexible schema
* universal JSON documents
* query data anyway
* distributed scalable database - horizontal scaling

## CAP model for MongoDB

![001_CAP-mongo](images/001_CAP-mongo.png)


## SQL Terms vs Mongo DB Terms

![002_CAP-mongo.png](images/002_CAP-mongo.png)

## JSON vs BSON

![003_CAP-mongo.png](images/003_CAP-mongo.png)

## Replica Set

![029_replica_set.png](images/029_replica_set.png)

## Causal consistency 

> "Causal consistency captures the potential causal relationships between operations, and guarantees that all processes observe causally-related operations in a common order. In other words, all processes in the system agree on the order of the causally-related operations. They may disagree on the order of operations that are causally unrelated.   
> 
> Let us define the following relation. If some process performs a write operation A, and some (the same or another) process that observed A then performs a write operation B, then it is possible that A is the cause of B; we say that A “potentially causes” or “causally precedes” B. Causal Consistency guarantees that if A causally-precedes B, then every process in the system observes A before observing B. Conversely, two write operations C and D are said concurrent, or causally independent, if neither causally precedes the other. In this case, a process may observe either C before D, or D before C"

> "Causal consistency can solve many problems, which cannot be solved in the eventual consistency, such as ordering operations. The causal consistency ensures that every sees operations in the same causal order, and this makes the causal consistency stronger than the eventual consistency"

# Setting up MongoDB Atlas

Go to https://www.mongodb.com/ and next click "Try Free" and next sign in for example using Google account.

## Create a new project and next go to this project

![004_create_a_new_project.png](images/004_create_a_new_project.png)

## Create a free cluster

![005_create_a_new_database.png](images/005_create_a_new_database.png)

Select cloud provider and region:

![006_create_a_new_database.png](images/006_create_a_new_database.png)

![007_create_a_new_database.png](images/007_create_a_new_database.png)

![008_create_a_new_database.png](images/008_create_a_new_database.png)

![009_create_a_new_database.png](images/009_create_a_new_database.png)

![010_create_a_new_database.png](images/010_create_a_new_database.png)

![011_create_a_new_database.png](images/011_create_a_new_database.png)

![012_create_a_new_database.png](images/012_create_a_new_database.png)

![013_create_a_new_database.png](images/013_create_a_new_database.png)


Connection string: `mongodb+srv://kicaj:<password>@myfirstcluster.a6uds.mongodb.net/test`

Download MongoDB Compass and run it (it does not require installation):   


![014_create_a_new_database.png](images/014_create_a_new_database.png)


![015_create_a_new_database.png](images/015_create_a_new_database.png)

### Load sample data

![016_load_sample_data.png](images/016_load_sample_data.png)

![017_load_sample_data.png](images/017_load_sample_data.png)

![018_load_sample_data.png](images/018_load_sample_data.png)


### Monitor create cluster

![019_monitoring.png](images/019_monitoring.png)

![020_monitoring.png](images/020_monitoring.png)

### View sample data

In Web Browser:

![021_view-sample-data.png](images/021_view-sample-data.png)


In MongoDB Compass:

![022_view-sample-data.png](images/022_view-sample-data.png)

![023_view-sample-data.png](images/023_view-sample-data.png)

### DB access

To get access to mongo DB make sure that your IP address is added to the whitelist:

![035_network_access.png](images/035_network_access.png)


# CRUD operations

* Create: `db.collection.insertOne(), db.collection.insertMany()`
* Read: `db.collection.find()`
* Update: `db.collection.updateOne()`
* Delete: `db.collection.deleteOne()`

## Create

* All write operations in MongoDB are atomic on the level of a single document even if it writes also to multiple embedded sub documents. It means that if we use `db.collection.insertMany()` and single document operation is atomic but the whole operation is not atomic. If we need atomicity for multiple documents that we have to use distributed transactions.

* If the collection does not currently exist insert operations will create the collection (table).

* If an inserted document omits the `_id` field, the MongoDB driver automatically generates and `ObjectId` for the `_id` field.
`_id` field must be unique in the collection otherwise  exception is thrown.

### Create using MongoDB Compass

First create database - to create database you have to also specify one collection name (in db there can be multiple collections).

![024_create_document.png](images/024_create_document.png)

![025_create_document.png](images/025_create_document.png)

![026_create_document.png](images/026_create_document.png)

Created data:
![027_create_document.png](images/027_create_document.png)

### Create using command line

First download MongoDB Shell for windows. Installation is not needed, just placed it in some folder and add it to the PATH variable.

![028_create_document.png](images/028_create_document.png)

Next connect to the mongo cluster:

```
D:\>mongosh "mongodb+srv://kicaj:kicaj@myfirstcluster.a6uds.mongodb.net/test"
Current Mongosh Log ID: 60fa8c09a58b733e1ccd8838
Connecting to:          mongodb+srv://<credentials>@myfirstcluster.a6uds.mongodb.net/test
Using MongoDB:          4.4.7
Using Mongosh:          1.0.1

For mongosh info see: https://docs.mongodb.com/mongodb-shell/


To help improve our products, anonymous usage data is collected and sent to MongoDB periodically (https://www.mongodb.com/legal/privacy-policy).
You can opt-out by running the disableTelemetry() command.

Atlas atlas-mritki-shard-0 [primary] test>                                                                              
```

To check to which DB you are connected run:

```
Atlas atlas-mritki-shard-0 [primary] test> db
test
```

`test` database it is an empty db that`s why it is not displayed in the Compass client, more info [here](https://www.mongodb.com/community/forums/t/no-test-database-is-shown-in-the-compass/92398/3).

Also if we do not specify DB name in connection string by default it connects to the `test` database:

```
D:\>mongosh "mongodb+srv://kicaj:kicaj@myfirstcluster.a6uds.mongodb.net"
Current Mongosh Log ID: 60fa8d55fecbf40cb220486a
Connecting to:          mongodb+srv://<credentials>@myfirstcluster.a6uds.mongodb.net/
Using MongoDB:          4.4.7
Using Mongosh:          1.0.1

For mongosh info see: https://docs.mongodb.com/mongodb-shell/

Atlas atlas-mritki-shard-0 [primary] test> db
```

To list all databases use command `show dbs`:

```
Atlas atlas-mritki-shard-0 [primary] test> show dbs
SQLAuthority          41 kB
sample_airbnb       54.9 MB
sample_analytics    9.94 MB
sample_geospatial    983 kB
sample_mflix        46.5 MB
sample_restaurants  6.21 MB
sample_supplies      983 kB
sample_training     46.3 MB
sample_weatherdata  2.96 MB
admin                340 kB
local               2.48 GB
Atlas atlas-mritki-shard-0 [primary] test>  
```

#### Create a new database using command line

> Always use lower case when you are using MongoDB command line.

For example this will fail:

```
Atlas atlas-mritki-shard-0 [primary] test> USE sqlauthoritynew
Uncaught:
SyntaxError: Unexpected token, expected ";" (1:4)

> 1 | USE sqlauthoritynew
    |     ^
  2 |

```

but this will create and select the created database:

```
Atlas atlas-mritki-shard-0 [primary] test> use sqlauthoritynew
switched to db sqlauthoritynew
Atlas atlas-mritki-shard-0 [primary] sqlauthoritynew>
```

When we list all dbs we see that this new db is missing on the list (similar to `test` db). It works like this because this new DB exists for now only in memory and if we close the session it will completely disappear. To "commit" the database we have to create at least one collection, then this new db will be also listed by `show dbs`:

```
Atlas atlas-mritki-shard-0 [primary] sqlauthoritynew> db
sqlauthoritynew
Atlas atlas-mritki-shard-0 [primary] sqlauthoritynew> db.createCollection("newusers")
{ ok: 1 }
Atlas atlas-mritki-shard-0 [primary] sqlauthoritynew> show collections
newusers
Atlas atlas-mritki-shard-0 [primary] sqlauthoritynew> show dbs
SQLAuthority          41 kB
sample_airbnb       54.9 MB
sample_analytics    9.94 MB
sample_geospatial    983 kB
sample_mflix        46.5 MB
sample_restaurants  6.21 MB
sample_supplies      983 kB
sample_training     46.3 MB
sample_weatherdata  2.96 MB
sqlauthoritynew     8.19 kB
admin                340 kB
local               2.48 GB
Atlas atlas-mritki-shard-0 [primary] sqlauthoritynew>
```

#### insertOne

```
Atlas atlas-mritki-shard-0 [primary] sqlauthoritynew> db.newusers.insertOne(
... {
.....     "DisplayName": "Pinal Dave",
.....     "UserName": "pinaldave",
.....     "Job": {
.......         "Title": "DBA",
.......         "Area": "Database Performance Tuning",
.......         "isManager": "false"
.......     },
.....     "Programming Languages": ["T-SQL", "JavaScript", "HTML"]
..... }
... )
{
  acknowledged: true,
  insertedId: ObjectId("60fac1a9d31016f4c75f09e4")
}
Atlas atlas-mritki-shard-0 [primary] sqlauthoritynew>
```

* `acknowledged: true` means that data has been successfully replicated to all nodes ? (CONFIRM IT).
* `insertedId`: in case we do not provide id value then it is auto generated by MongoDB

##### Reading table

```
Atlas atlas-mritki-shard-0 [primary] sqlauthoritynew> db.newusers.find( {} ).pretty()
[
  {
    _id: ObjectId("60fac1a9d31016f4c75f09e4"),
    DisplayName: 'Pinal Dave',
    UserName: 'pinaldave',
    Job: {
      Title: 'DBA',
      Area: 'Database Performance Tuning',
      isManager: 'false'
    },
    'Programming Languages': [ 'T-SQL', 'JavaScript', 'HTML' ]
  }
]
```


#### insertMany

```
db.newusers.insertMany(
[
{
    "DisplayName": "Pinal Dave",
    "UserName": "pinaldave",
    "Job": {
        "Title": "DBA",
        "Area": "Database Performance Tuning",
        "isManager": "false"
    },
    "Programming Languages": ["T-SQL", "JavaScript", "HTML"]
},
{
    "DisplayName": "Jason Brown",
    "UserName": "jasonbrown",
    "Job": {
        "Title": "DBA",
        "Area": "Database Performance Tuning",
        "isManager": "true"
    },
    "Programming Languages": ["T-SQL", "JavaScript", "HTML"]
},
{
    "DisplayName": "Mark Smith",
    "UserName": "marksmith",
    "Job": {
        "Title": "DBA",
        "Area": "Database Development",
        "isManager": "false",
	"YearsExp":{"$numberInt":"5"}
    },
    "Programming Languages": ["T-SQL", "JavaScript"]
}
]
)
```

## Reading data

### Query and query operators

* comparison
* logical
* element
* evaluation
* geo-spatial
* array
* bitwise

More in [docs](https://docs.mongodb.com/manual/reference/operator/query/).

### Query projections

Defines which fields should be returned by the query.

* 1 or true: include the field
* 0 or false: exclude the field

### Read concerns

More in [docs](https://docs.mongodb.com/manual/reference/read-concern/).

Allows to control the consistency and isolation properties of the data read from replica sets and replica set shards.

* Local: no guarantee that the data has been written to majority of the replica set members. The data that you are reading might be rolled back. It is default mode when we read from the primary replica set.

* Available: the same local but it is default when we read from the secondary replica set. 

* Majority: **this is the default for all the fix operators if you do not specify and of the read concerns.** It is default read concern for `db.find` function. In this case the query returns a data that has been acknowledged by the majority of the replica set members. The documents returned by the read operation are durable, even in the event of failure.

* Linearisible: the query returns data that reflects all successful majority-acknowledged writes that completed prior to the start of the read operation. The query may wait for concurrently executing writes to propagate to a majority of replica set members before returning results.   
  You can specify linearizable read concern for read operations on the primary only.

  Linearizable read concern guarantees only apply if read operations specify a query filter that uniquely identifies a single document.

* Snapshot
  * If the transaction is not part of a causally consistent session, upon transaction commit with write concern "majority", the transaction operations are guaranteed to have read from a snapshot of majority-committed data.
  * If the transaction is part of a causally consistent session, upon transaction commit with write concern "majority", the transaction operations are guaranteed to have read from a snapshot of majority-committed data that provides causal consistency with the operation immediately preceding the transaction start.

### Reading using MongoDB compass

![030_filter_in_compass.png](images/030_filter_in_compass.png)

![031_filter_in_compass.png](images/031_filter_in_compass.png)

Returns all movies where `Billy Bletcher` cast:

![032_filter_in_compass.png](images/032_filter_in_compass.png)

![033_filter_in_compass.png](images/033_filter_in_compass.png)

Another examples:

`{ $and: [{runtime: {$gt: 80}}, {"awards.wins": 3}]}`   

`{ $or: [{runtime: {$gt: 80}}, {"awards.wins": 3}]}`   

Using options:

![034_filter_in_compass.png](images/034_filter_in_compass.png)

### Reading using command line

Connect to DB:

```
D:\>mongosh "mongodb+srv://kicaj:kicaj@myfirstcluster.a6uds.mongodb.net"
Current Mongosh Log ID: 60fe69b45196251cb4d2261e
Connecting to:          mongodb+srv://<credentials>@myfirstcluster.a6uds.mongodb.net/
Using MongoDB:          4.4.7
Using Mongosh:          1.0.1

For mongosh info see: https://docs.mongodb.com/mongodb-shell/

Atlas atlas-mritki-shard-0 [primary] test>
```

#### select existing DB
```
use sample_mflix
```

#### list of collections
```
show collections
```

#### query all movies
```
db.movies.find({}).pretty()
```

#### returns next set of results
```
it
```

#### pretty

```
db.movies.find( {runtime: 11} ).pretty()
db.movies.find( {runtime: 11} ).pretty().limit(3)
```

#### projection (selecting which fields should be displayed):
```
db.movies.find( {runtime: 11}, {runtime:1, title:1, _id:0} ).pretty().limit(3)
```

#### sorting
```
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find( {runtime: {$eq: 11}}, {runtime:1, title:1, _id:0} ).pretty().limit(3).sort({title: 1})
[
  { runtime: 11, title: '9' },
  { runtime: 11, title: 'A Is for Autism' },
  { runtime: 11, title: 'Apricot' }
]

Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find( {runtime: {$eq: 11}}, {runtime:1, title:1, _id:0} ).pretty().limit(3).sort({title: -1})
[
  { runtime: 11, title: 'Your Friend the Rat' },
  { runtime: 11, title: 'The Portal' },
  {
    runtime: 11,
    title: 'The Life and Death of 9413, a Hollywood Extra'
  }
]
```

#### specifying read concerns
```
db.movies.find( {runtime: {$eq: 11}}, {runtime:1, title:1, _id:0} ).pretty().limit(3).sort({title: -1}).readConcern("majority")
```

```
db.movies.find( {runtime: {$eq: 11}}, {runtime:1, title:1, _id:0} ).pretty().limit(3).sort({title: -1}).readConcern("linearizable").maxTimeMS(10000)
```

>NOTE: in case very busy system when we run both above queries the second query might return different data then the first query because `linearizable` only returns the data after all the previous write operations commit data into all replicas. QUESTION: really all replicas or majority? [Docs](https://docs.mongodb.com/manual/reference/read-concern-linearizable/#mongodb-readconcern-readconcern.-linearizable-) says that it is 'https://docs.mongodb.com/manual/reference/read-concern-linearizable/#mongodb-readconcern-readconcern.-linearizable-'. Also docs says: "  Linearizable read concern guarantees only apply if read operations specify a query filter that uniquely identifies a single document."

## Write concerns

It specifies level of acknowledgement requested from MongoDB for write operations. [Docs](https://docs.mongodb.com/manual/reference/write-concern/).

* w:1 - ack from primary.  Data can be rolled back if the primary steps down before the write operations have replicated to any of the secondaries.
* w:0 - no ack. However, w: 0 may return information about socket exceptions and networking errors to the application. Data can be rolled back if the primary steps down before the write operations have replicated to any of the secondaries.
* w:(n) - primary + (n-1) secondary. For example if we have replica set: primary and 2 secondaries then `w:2` then we get ack from primary and one of the secondaries, `w:3` we will get ack from all nodes.
* w: majority. Requests acknowledgment that write operations have propagated to the calculated majority of the data-bearing voting members (i.e. primary and secondaries with members[n].votes greater than 0). w: majority is the default write concern for most MongoDB configurations. See Implicit Default Write Concern. For example, consider a replica set with 3 voting members, Primary-Secondary-Secondary (P-S-S). For this replica set, calculated majority is two, and the write must propagate to the primary and one secondary to acknowledge the write concern to the client.
* wtimeout: time limit to prevent write operations from blocking indefinitely. Value 0 means infinity. `wtimeout` is only applicable for w values greater than 1.

## Update

Operations:
* db.collection.updateOne()
* db.collection.updateMany()
* db.collection.replaceOne() - it entirely replaces the whole document but `_id` stays the same!

Important points:

* atomic on the level of a single document - so `updateMany` can be partially succeeded.
* `_id` field cannot be replace with different value.
* `$set` creates a field if not already existing.
* `upsert: true` - if a document match the query it will be updated and if nothing is found that a new document will be created.


### Update in MongoDB Compass

![036_edit_document.png](images/036_edit_document.png)

### Update in command line

#### Update single document

```
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find( {runtime: {$eq: 12}}, {runtime:1, title:1, year:1, _id:0} ).pretty().limit(3).sort({title: -1})
[
  { runtime: 12, title: 'The OldestCrocodile', year: 2005 },
  { runtime: 12, title: 'The Necktie', year: 2008 },
  { runtime: 12, title: 'The House of Small Cubes', year: 2008 }
]
Atlas atlas-mritki-shard-0 [primary] sample_mflix>  db.movies.updateOne(
...     { title: {$eq: "The OldestCrocodile"}},
...     {
.....           $set: {"title":"The Very New Crocodile", "year": 2020}
.....   }
... )
{
  acknowledged: true,
  insertedId: null,
  matchedCount: 1,
  modifiedCount: 1,
  upsertedCount: 0
}
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find( {runtime: {$eq: 12}}, {runtime:1, title:1, year:1, _id:0} ).pretty().limit(3).sort({title: -1})
[
  { runtime: 12, title: 'The Very New Crocodile', year: 2020 },
  { runtime: 12, title: 'The Necktie', year: 2008 },
  { runtime: 12, title: 'The House of Small Cubes', year: 2008 }
]
```

>NOTE: MongoDB is case sensitive, for example if field name is `Year` then `$set: {"year": 2020}` will not update the year but it will create a new field `Year` ! 
```
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find( {runtime: {$eq: 12}}, {runtime:1, title:1, year:1, Year:1, _id:0} ).pretty().limit(3).sort({title: -1})
[
  {
    runtime: 12,
    title: 'The Very New Crocodile',
    year: 2020,
    Year: 2020
  },
  { runtime: 12, title: 'The Necktie', year: 2008 },
  { runtime: 12, title: 'The House of Small Cubes', year: 2008 }
]
```

#### Update multiple documents

```
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find({year: {$eq: 1988}}).count()
265
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find({year: {$eq: 2025}}).count()
0
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.updateMany(
...     { year: {$eq: 1988}},
...     {
.....     $set: {"year": 2025}
.....   }
... )
{
  acknowledged: true,
  insertedId: null,
  matchedCount: 265,
  modifiedCount: 265,
  upsertedCount: 0
}
```

##### Using `upsert`

This example inserts **one document** with year 1988 but next it is updated to value 2025.
```
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find({year: {$eq: 1988}}).count()
0
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find({year: {$eq: 2025}}).count()
265
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.updateMany(
...     { year: {$eq: 1988}},
...     {
.....     $set: {"year": 2025}
.....   },
...     { upsert: true }
... )
{
  acknowledged: true,
  insertedId: ObjectId("60feba97b66ffeeafd3496d4"),
  matchedCount: 0,
  modifiedCount: 0,
  upsertedCount: 1
}
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find({year: {$eq: 1988}}).count()
0
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find({year: {$eq: 2025}}).count()
266
```

In this example we insert a document with year set on 1988 and next we update this document by adding new fields `title` and `awards.wins`:

```
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find({year: {$eq: 2025}}).count()
266
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find({year: {$eq: 1988}}).count()
0
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.updateMany(
...     { year: {$eq: 1988}},
...     {
.....     $set: {"title": "MySuperFunnyTitle", "awards.wins": 9}
.....   },
...     { upsert: true }
... )
{
  acknowledged: true,
  insertedId: ObjectId("60febd8bb66ffeeafd3c9231"),
  matchedCount: 0,
  modifiedCount: 0,
  upsertedCount: 1
}
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find({year: {$eq: 1988}}).count()
1
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find({year: {$eq: 2025}}).count()
266
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find({year: {$eq: 1988}}).pretty()
[
  {
    _id: ObjectId("60febd8bb66ffeeafd3c9231"),
    year: 1988,
    awards: { wins: 9 },
    title: 'MySuperFunnyTitle'
  }
]
```

#### Specify write concern

```
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.updateMany(
...     { runtime: {$eq: 1122}},
...     {
.....     $set: {"title": "MySuperFunnyTitle", "Year": 2020, "awards.wins": 9}
.....   },
...     { upsert: true, w:"majority", wtimeout: 1000 }
... )
{
  acknowledged: true,
  insertedId: ObjectId("60febefcb66ffeeafd408669"),
  matchedCount: 0,
  modifiedCount: 0,
  upsertedCount: 1
}
```

### Replace command

```
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find( {runtime: 1122 }).pretty()
[
  {
    _id: ObjectId("60febefcb66ffeeafd408669"),
    runtime: 1122,
    Year: 2020,
    awards: { wins: 9 },
    title: 'MySuperFunnyTitle'
  }
]
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.replaceOne(
...     {runtime: {$eq: 1122}},
...     {
.....      runtime: 1122,
.....      "NoTitile": "ReplaceOneExample",
.....      "NewYear": 2020,
.....      "awards.losts": 5
.....   }
... )
{
  acknowledged: true,
  insertedId: null,
  matchedCount: 1,
  modifiedCount: 1,
  upsertedCount: 0
}
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find( {runtime: 1122 }).pretty()
[
  {
    _id: ObjectId("60febefcb66ffeeafd408669"),
    runtime: 1122,
    NoTitile: 'ReplaceOneExample',
    NewYear: 2020,
    'awards.losts': 5
  }
]
```

## Delete

* all write operations in MongoDB are atomic on the level of single document
* delete does not drop indexes - they have to be explicitly deleted

Operations:

* db.collection.deleteOne()
* db.collection.deleteMany()
* db.collection.remove()

### Delete in MongoDB Compass

![037_delete_in_compass.png](images/037_delete_in_compass.png)

![038_delete_in_compass.png](images/038_delete_in_compass.png)

### Delete in command line

#### Delete one

```
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find( {runtime: 25} ).count()
13
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.deleteOne( {runtime: 25} )
{ acknowledged: true, deletedCount: 1 }
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find( {runtime: 25} ).count()
12
```

#### Delete many

```
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find( {runtime: 25} ).count()
12
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.deleteMany( {runtime: 25} )
{ acknowledged: true, deletedCount: 12 }
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find( {runtime: 25} ).count()
0
```

#### Remove

In general remove works the same as delete but it returns different information are the result of this operation.

Docs say that [delete](https://docs.mongodb.com/manual/reference/method/db.collection.remove/) and [remove](https://docs.mongodb.com/manual/reference/method/db.collection.remove/) returns different result types

delete:
```
{
  acknowledged: true/false,
  deletedCount: n
}
```
remove:
```
{ "nRemoved" : n }
```

but in my examples result schema is always the same so maybe there is no difference.

More about `remove` [here](https://groups.google.com/g/mongodb-user/c/v5JavfJcKQk?pli=1).


>"The deleteMany() method was added in MongoDB 3.2 to make the mongo shell CRUD API consistent with the driver API. The remove() method is the historical interface which is available for backward compatibility."



```
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find( {runtime: 35} ).count()
8
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.remove( {runtime: 35}, true )
{ acknowledged: true, deletedCount: 1 }
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find( {runtime: 35} ).count()
7
```

Used parameter `true` tells to remove only single document within a collection.


If we do not use this parameter then all matched documents will be deleted:

```
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find( {runtime: 35} ).count()
7
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.remove( {runtime: 35})
{ acknowledged: true, deletedCount: 7 }
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find( {runtime: 35} ).count()
0
```

To remove all documents from a collection:

```
db.movies.remove({})
```

# 'Foreign key constraint' in MongoDB

https://www.mongodb.com/community/forums/t/foreign-key-constraint/5814