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
      - [sorting and limit](#sorting-and-limit)
      - [findOne](#findone)
      - [filtering by date](#filtering-by-date)
      - [comparing arrays](#comparing-arrays)
        - [array query operators](#array-query-operators)
        - [array projection operators](#array-projection-operators)
      - [comparing objects](#comparing-objects)
      - [comparing floating points and integers](#comparing-floating-points-and-integers)
      - [$in and $nin](#in-and-nin)
        - [$in with array field](#in-with-array-field)
        - [$in and regex](#in-and-regex)
      - [logical query operators AND/OR and between](#logical-query-operators-andor-and-between)
      - [querying nested documents](#querying-nested-documents)
      - [querying null fields, missing field and field types](#querying-null-fields-missing-field-and-field-types)
        - [querying null fields and checking if field exist](#querying-null-fields-and-checking-if-field-exist)
        - [querying by checking field types](#querying-by-checking-field-types)
      - [specifying read concerns](#specifying-read-concerns)
      - [free text search](#free-text-search)
        - [full text search relevance](#full-text-search-relevance)
      - [Sample queries](#sample-queries)
    - [Cursor](#cursor)
  - [Write concerns](#write-concerns)
  - [Update](#update)
    - [Update in MongoDB Compass](#update-in-mongodb-compass)
    - [Update in command line](#update-in-command-line)
      - [Update single document](#update-single-document)
      - [Update multiple documents](#update-multiple-documents)
        - [Using `upsert`](#using-upsert)
      - [Specify write concern](#specify-write-concern)
    - [Replace command](#replace-command)
    - [Updating multiple records in C-Sharp](#updating-multiple-records-in-c-sharp)
  - [Delete](#delete)
    - [Delete in MongoDB Compass](#delete-in-mongodb-compass)
    - [Delete in command line](#delete-in-command-line)
      - [Delete one](#delete-one)
      - [Delete many](#delete-many)
      - [Remove](#remove)
- ['Foreign key constraint' in MongoDB](#foreign-key-constraint-in-mongodb)
- [Relational SQL vs Document DB](#relational-sql-vs-document-db)
- [Common SQL concepts and semantics to MongoDB](#common-sql-concepts-and-semantics-to-mongodb)
  - [Create table/collection](#create-tablecollection)
  - [Add columns/fields](#add-columnsfields)
  - [Create index](#create-index)
  - [Insert statement](#insert-statement)
  - [Select statement](#select-statement)
  - [Select statement - filter](#select-statement---filter)
  - [Update statement](#update-statement)
  - [Delete statement](#delete-statement)
  - [Drop table/collection](#drop-tablecollection)
- [Importing.exporting data](#importingexporting-data)
  - [Importing](#importing)
  - [Exporting](#exporting)
- [References/docs](#referencesdocs)
- [Oplog](#oplog)
  - [Oplog stats](#oplog-stats)
  - [replication info](#replication-info)
  - [replication info on secondary nodes](#replication-info-on-secondary-nodes)
  - [replication service status](#replication-service-status)
- [Backup and restore](#backup-and-restore)
- [Aggregations](#aggregations)
- [AspNetCore and MongoDB](#aspnetcore-and-mongodb)

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

* Consistency: Every read receives the most recent write or an error. **IT IS NOT CONSISTENCE BETWEEN DOCUMENTS RELATIONS BUT CONSISTENCY BETWEEN NODES!**
* Availability: Every request receives a (non-error) response, without the guarantee that it contains the most recent write
* Partition tolerance: The system continues to operate despite an arbitrary number of messages being dropped (or delayed) by the network between nodes

When a network partition failure happens should we decide to:

* Cancel the operation and thus decrease the availability but ensure consistency
* Proceed with the operation and thus provide availability but risk inconsistency

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

By default mongodb splits the output of batches of 20 objects.
To navigate to the next batch type `it`.

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

> If we specify columns with only 0 value then all not listed columns will be returned.

#### sorting and limit

> MongoDB does not guarantee the order of the returned documents unless you use sort().

Sort is always executed before limit, even if we write `cursor.limit().sort()` MongoDbB will execute `cursor.sort().limit()`.
Using only limit usually does not make sense because we will get data in random order.

```
Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find( {runtime: {$eq: 11}}, {runtime:1, title:1, _id:0} ).sort({title: 1}).limit(3).pretty()
[
  { runtime: 11, title: '9' },
  { runtime: 11, title: 'A Is for Autism' },
  { runtime: 11, title: 'Apricot' }
]

Atlas atlas-mritki-shard-0 [primary] sample_mflix> db.movies.find( {runtime: {$eq: 11}}, {runtime:1, title:1, _id:0} ).sort({title: -1}).limit(3).pretty()
[
  { runtime: 11, title: 'Your Friend the Rat' },
  { runtime: 11, title: 'The Portal' },
  {
    runtime: 11,
    title: 'The Life and Death of 9413, a Hollywood Extra'
  }
]
```
```
db.trips.find({"birth year": { $ne: ''}}, {"birth year": 1}).sort({"birth year": -1}).limit(1)
```

#### findOne

```
db.collection.findOne([query], [projection]): document
```

* by default, it returns the document that matches the criteria
* if multiple documents match the criteria, the method returns the first document according to the order the documents are stored on disk
* if no documents match the criteria, the method returns null

Methods from `cursor` object like `pretty` or `count` are not available here because returned data type is `document` and not an object.

#### filtering by date

```
db.flights.find({departureDate: ISODate("2020-02-20T23:00:00Z")})
```
or we can create an object:
```
db.flights.find({departureDate: ISODate("2020-02-20T23:00:00Z")})
```

If we specify only date then implicitly time value 12am is appended:
```
db.flights.find({departureDate: ISODate("2020-02-20")})
```

but we can use for example `$lt` or `gt` and skip time in comparison:

```
db.flights.find({departureDate: { $gt: ISODate("2020-02-20")} })
```

```
{ "birth year": 1961, "start station name": "Howard St & Centre St" }

{ "CreationDate": {$lt: new Date('2022-01-25')} }

{ $and: [ { "CreationDate": {$lt: new Date('2022-01-25')} } ] }

{ $and: [ { "CreationDate": {$lt: new Date('2022-01-25')} }, { "State": 100 } ] }
 
{ $and: [ { "CreationDate": {$lt: new Date('2022-01-26')} }, { "State": "Processing" } ] }
 
{ $and: [ { "CreationDate": {$lt: new Date('2022-01-26')} }, { "State": 100 } ] }
 
// This one is working in mongo db client web ui atlas:
{ $and: [ { "CreationDate": {$lt: ISODate('2021-07-03')} }, { "State": 100 } ] }

db.companies.find( { "created_at" : { "$gte": ISODate("2007-01-01"), "$lte": ISODate("2007-12-31")} }, { "created_at": 1 } )

db.companies.find( { 
  $or: [
    { $and: [ { "founded_year" : 2004 }, { "category_code": { $in: ["social", "web"] } } ] },
    { $and: [ { "founded_month" : 10 }, { "category_code": { $in: ["social", "web"] } } ] }
  ]}, 
  { "founded_year": 1, "founded_month": 1, "category_code": 1 } )
```

#### comparing arrays

>By default arrays are the same if they have the same elements in the same order. So it has to exact match to be returned by the query.

```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.crew.find({skills: ["engineering"]})
[
  {
    _id: ObjectId("60ffffa5f723cd527c447f43"),
    name: 'Gunter Hoff',
    skills: [ 'engineering' ],
    address: { city: 'Berlin', country: 'Germany' }
  }
]
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.crew.find({skills: ["technical", "management"]})
[
  {
    _id: ObjectId("60ffffa5f723cd527c447f44"),
    name: 'Andrei Luca',
    skills: [ 'technical', 'management' ],
    address: { city: 'Bucharest', country: 'Romania' }
  },
  {
    _id: ObjectId("60ffffa5f723cd527c447f45"),
    name: 'Anna Smith',
    skills: [ 'technical', 'management' ],
    address: { city: 'Bucharest', country: 'Romania' }
  }
]
```

>To create a condition where any of a array values is equal to the condition value use `find` without `[]`.

```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.crew.find({skills: "technical"})
[
  {
    _id: ObjectId("61014b1c5b94f0bdbef0accb"),
    name: 'Anna Smith',
    skills: [ 'technical', 'management' ],
    address: { city: 'Bucharest', country: 'Romania' }
  },
  {
    _id: ObjectId("61014b1c5b94f0bdbef0accc"),
    name: 'Andrei Luca',
    skills: [ 'technical', 'management' ],
    address: { city: 'Bucharest', country: 'Romania' }
  }
]
```

If you use `[]` then nothing will be returned.
```
db.crew.find({skills: ["technical"]})
```

##### array query operators

* $all: matches arrays that contain all the elements in a provided list, **regardless of the order**
```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.crew.find({skills: {$all: ["management", "technical"]}})
[
  {
    _id: ObjectId("61014b1c5b94f0bdbef0accb"),
    name: 'Anna Smith',
    skills: [ 'technical', 'management' ],
    address: { city: 'Bucharest', country: 'Romania' }
  },
  {
    _id: ObjectId("61014b1c5b94f0bdbef0accc"),
    name: 'Andrei Luca',
    skills: [ 'technical', 'management' ],
    address: { city: 'Bucharest', country: 'Romania' }
  }
]
```
* $size: matches arrays where the size is equal to the given value
```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.crew.find({skills: {$size: 2}})
[
  {
    _id: ObjectId("61014b1c5b94f0bdbef0accb"),
    name: 'Anna Smith',
    skills: [ 'technical', 'management' ],
    address: { city: 'Bucharest', country: 'Romania' }
  },
  {
    _id: ObjectId("61014b1c5b94f0bdbef0accc"),
    name: 'Andrei Luca',
    skills: [ 'technical', 'management' ],
    address: { city: 'Bucharest', country: 'Romania' }
  }
]
```

Another example: `{"amenities": { "$size": 32, "$all": ["Wifi", "Kitchen"] }}`. This will return only these documents that have 32 amenities and they always have
Wifi and Kitchen.

* $elemMatch: returns arrays where an element matches multiple conditions

For example return all flights where in a crew is entry with "Gotthard Merz" and "hoursSlept" equal 6.

```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.flights.find({crew: {$elemMatch: {name: "Gotthard Merz", hoursSlept: 6} }})
[
  {
    _id: ObjectId("60fffedb66fa675f8e5f8869"),
    type: 'Intercontinental',
    delayed: false,
    duration: 600,
    departureDate: ISODate("2020-03-20T18:10:00.000Z"),
    distanceKm: 6400,
    departure: {
      code: 'MUC',
      city: 'Munich',
      country: 'Germany',
      runwayLength: 4000,
      location: { type: 'Point', coordinates: [ 11.7, 48.3 ] }
    },
    destination: {
      code: 'JFK',
      city: 'New York',
      country: 'USA',
      runwayLength: 4423,
      location: { type: 'Point', coordinates: [ -74, 40.7 ] }
    },
    aircraftCode: '00126a63-f342-4ccd-ba86-4a7beecf10c0',
    crew: [
      { name: 'Marcel Danzig', position: 'Captain', hoursSlept: 7 },
      {
        name: 'Gotthard Merz',
        position: 'Attendant',
        nationality: 'Germany',
        hoursSlept: 6
      },
      {
        name: 'Bastian Steyer',
        position: 'Attendant',
        nationality: 'Germany',
        hoursSlept: 3
      },
      {
        name: 'Isabell Jahnke',
        position: 'Attendant',
        nationality: 'Germany',
        hoursSlept: 5
      }
    ]
  }
]
```


```
 db.listingsAndReviews.find({ "$and": [ { "accommodates": { "$gt": 6 } }, { "reviews": { "$size": 50 } } ] }).count()
 db.listingsAndReviews.find({ "$and": [ { "accommodates": { "$gt": 6 } }, { "reviews": { "$size": 50 } } ] }, { "name": 1 })
 ```
 Filter `{ "amenities": "Changing table" }` will return listingsAndReviews that have a string field `amenities` with value `Changing table` or a documents
 that have array `amenities` that contains `Changing table` so it is not the same as `$in` operator.
 ```
 db.listingsAndReviews.find({ "$and": [ { "property_type": "House" }, { "amenities": "Changing table" } ] }).count()
 db.listingsAndReviews.find({ "$and": [ { "property_type": "House" }, { "amenities": { "$in": ["Changing table"] } } ] }).count()
 db.companies.find({ "offices": { "$elemMatch": { "city": "Seattle" } } }).count()
 db.companies.find({ "relationships.0.person.last_name": "Zuckerberg" }, { "name": 1 }).pretty()
 db.companies.find({ "relationships.0.person.first_name": "Mark", "relationships.0.title": { "$regex": "CEO" } }, { "name": 1 }).pretty()
 db.companies.find({ "relationships": { "$elemMatch": { "is_past": true, "person.first_name": "Mark" } } }, { "name": 1 } ).pretty()
 db.trips.find({ "start station location.coordinates.0": { "$eq": -74.015756 } }).pretty()
 db.trips.find({ "start station location.coordinates.0": { "$lt": -74 } }).pretty()
 db.inspections.find({  "address.city": "NEW YORK" }).count()

```

##### array projection operators

Can be used to determine which elements of an array should be returned or hidden from a query.

* $slice: limits the number of items in an array that a query returns

Return only first element from the crew array:
```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.flights.find({crew: {$elemMatch: {name: "Gotthard Merz", hoursSlept: 6}}}, {crew: {$slice: 1}})
[
  {
    _id: ObjectId("60fffedb66fa675f8e5f8869"),
    type: 'Intercontinental',
    delayed: false,
    duration: 600,
    departureDate: ISODate("2020-03-20T18:10:00.000Z"),
    distanceKm: 6400,
    departure: {
      code: 'MUC',
      city: 'Munich',
      country: 'Germany',
      runwayLength: 4000,
      location: { type: 'Point', coordinates: [ 11.7, 48.3 ] }
    },
    destination: {
      code: 'JFK',
      city: 'New York',
      country: 'USA',
      runwayLength: 4423,
      location: { type: 'Point', coordinates: [ -74, 40.7 ] }
    },
    aircraftCode: '00126a63-f342-4ccd-ba86-4a7beecf10c0',
    crew: [ { name: 'Marcel Danzig', position: 'Captain', hoursSlept: 7 } ]
  }
]
```

Another version can skip n elements and next return m elements:

```
db.flights.find({}, {crew: {$slice: [1,1]}})
```

* $: given the fact that you have an array field in the query document, the $ operator limits the array content to the first N elements that matches the query condition

For example show first element that matches an array query:

```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.crew.find({skills: "management"}, {"skills.$": 1})
[
  {
    _id: ObjectId("61014b1c5b94f0bdbef0accb"),
    skills: [ 'management' ]
  },
  {
    _id: ObjectId("61014b1c5b94f0bdbef0accc"),
    skills: [ 'management' ]
  }
]
```

* $elemMatch: display only the first element to match the projection condition (so we can see that $elemMatch can be used as projection and also as filter).

For example show first element that matches the conditions:

```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.flights.find({}, {crew: { $elemMatch: { hoursSlept: {$gt: 8}} }})
[
  { _id: ObjectId("60fffedb66fa675f8e5f886d") },
  {
    _id: ObjectId("60fffedb66fa675f8e5f8866"),
    crew: [ { name: 'Adina Popescu', position: 'Attendant', hoursSlept: 9 } ]
  },
  { _id: ObjectId("60fffedb66fa675f8e5f8869") },
  { _id: ObjectId("60fffedb66fa675f8e5f886a") },
  { _id: ObjectId("60fffedb66fa675f8e5f886e") }
]

```

#### comparing objects

> Similar like for arrays here also both objects have to be exactly the same to be returned by the query (keys/values/order of keys). The second query will return no data.

```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.crew.find({address: {city: "Paris", country: "France"}})
[
  {
    _id: ObjectId("60ffffa5f723cd527c447f42"),
    name: 'Francois Picard',
    skills: [],
    address: { city: 'Paris', country: 'France' }
  }
]
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.crew.find({address: {city: "Paris"}})
```

#### comparing floating points and integers

> It looks that floating numbers are not rounded in comparisons.


```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.aircraft.find({ range: { $lt: 3219.45} })
[
  {
    _id: ObjectId("60fffe350dcd6a577496a797"),
    code: '0c3a60d6-8c99-472e-bf23-c1e689c5f6eb',
    model: 'ATR 72',
    minRunwayLength: 1000,
    range: 3218,
    capacity: 78
  }
]
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.aircraft.find({ range: { $lt: 3217.99} })

Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.aircraft.find({ range: { $lt: 3218.99} })
[
  {
    _id: ObjectId("60fffe350dcd6a577496a797"),
    code: '0c3a60d6-8c99-472e-bf23-c1e689c5f6eb',
    model: 'ATR 72',
    minRunwayLength: 1000,
    range: 3218,
    capacity: 78
  }
]
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.aircraft.find({ range: { $lt: 3218.01} })
[
  {
    _id: ObjectId("60fffe350dcd6a577496a797"),
    code: '0c3a60d6-8c99-472e-bf23-c1e689c5f6eb',
    model: 'ATR 72',
    minRunwayLength: 1000,
    range: 3218,
    capacity: 78
  }
]
```

#### $in and $nin

>$in: selects documents where the value of a field equals any value in a specified array.

>$nin: selects documents where the value of a field is not found in a specified array. 

```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.aircraft.find({model: {$in: ["Boeing 737-800", "Airbus A320"]}})
[
  {
    _id: ObjectId("60fffe350dcd6a577496a799"),
    code: 'eede6be6-f716-4e2e-bf81-885f0a16a50c',
    model: 'Boeing 737-800',
    minRunwayLength: 2500,
    range: 5765,
    capacity: 200
  },
  {
    _id: ObjectId("60fffe350dcd6a577496a79b"),
    code: '1b7ad0de-5836-489b-9791-5a81a51cdb81',
    model: 'Airbus A320',
    minRunwayLength: 2500,
    range: 6000,
    capacity: 150
  }
]
```

##### $in with array field

>If there is at least one value from the array in the object that is equal to one value from array filter then the object will be returned.

```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.crew.find({skills: {$in: ["technical", "engineering"]}})
[
  {
    _id: ObjectId("60ffffa5f723cd527c447f43"),
    name: 'Gunter Hoff',
    skills: [ 'engineering' ],
    address: { city: 'Berlin', country: 'Germany' }
  },
  {
    _id: ObjectId("60ffffa5f723cd527c447f44"),
    name: 'Andrei Luca',
    skills: [ 'technical', 'management' ],
    address: { city: 'Bucharest', country: 'Romania' }
  },
  {
    _id: ObjectId("60ffffa5f723cd527c447f45"),
    name: 'Anna Smith',
    skills: [ 'technical', 'management' ],
    address: { city: 'Bucharest', country: 'Romania' }
  }
]
```

##### $in and regex

Returns all crew objects where at least one skill starts from "te" or contains "ee".

```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.crew.find({skills: {$in: [/^te/, /ee/]}})
[
  {
    _id: ObjectId("60ffffa5f723cd527c447f43"),
    name: 'Gunter Hoff',
    skills: [ 'engineering' ],
    address: { city: 'Berlin', country: 'Germany' }
  },
  {
    _id: ObjectId("60ffffa5f723cd527c447f44"),
    name: 'Andrei Luca',
    skills: [ 'technical', 'management' ],
    address: { city: 'Bucharest', country: 'Romania' }
  },
  {
    _id: ObjectId("60ffffa5f723cd527c447f45"),
    name: 'Anna Smith',
    skills: [ 'technical', 'management' ],
    address: { city: 'Bucharest', country: 'Romania' }
  }
]
```

#### logical query operators AND/OR and between

```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.aircraft.find({ $and: [ {capacity: 150}, {range: {$gt: 5000}}] }).pretty()
[
  {
    _id: ObjectId("60fffe350dcd6a577496a79b"),
    code: '1b7ad0de-5836-489b-9791-5a81a51cdb81',
    model: 'Airbus A320',
    minRunwayLength: 2500,
    range: 6000,
    capacity: 150
  }
]
```

between:
```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.aircraft.find({ $and: [ {range: {$lt: 6050}},  {range: {$gt: 5900}}] }).pretty()
[
  {
    _id: ObjectId("60fffe350dcd6a577496a79b"),
    code: '1b7ad0de-5836-489b-9791-5a81a51cdb81',
    model: 'Airbus A320',
    minRunwayLength: 2500,
    range: 6000,
    capacity: 150
  }
]
```
another syntax of between:
```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.aircraft.find({range: {$lt: 6050, $gt: 5900}}).pretty()
[
  {
    _id: ObjectId("60fffe350dcd6a577496a79b"),
    code: '1b7ad0de-5836-489b-9791-5a81a51cdb81',
    model: 'Airbus A320',
    minRunwayLength: 2500,
    range: 6000,
    capacity: 150
  }
]
```

#### querying nested documents

> When you refer to nested objects then you have to use `.` and because of this the path has to be in `""`.

```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.crew.find({"address.city": "Berlin"}).pretty()
[
  {
    _id: ObjectId("60ffffa5f723cd527c447f43"),
    name: 'Gunter Hoff',
    skills: [ 'engineering' ],
    address: { city: 'Berlin', country: 'Germany' }
  }
]
```

#### querying null fields, missing field and field types


##### querying null fields and checking if field exist

* `''` and `null` are not equal

* querying for null fields will return docs where value is null or the field does not exist
```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.crew.find( {address: null} ).pretty();
[
  {
    _id: ObjectId("61014b1c5b94f0bdbef0acc6"),
    name: 'Antek2',
    skills: [ 'engineering' ]
  },
  {
    _id: ObjectId("61014b1c5b94f0bdbef0acc9"),
    name: 'Placek',
    skills: [ 'engineering' ],
    address: null
  }
]
```

* querying for documents where a field exist
```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.crew.find( {address: {$exists: false}} ).pretty();
[
  {
    _id: ObjectId("61014b1c5b94f0bdbef0acc6"),
    name: 'Antek2',
    skills: [ 'engineering' ]
  }
]
```

##### querying by checking field types

By familiar with BSON types: [docs](https://docs.mongodb.com/manual/reference/bson-types/). For example 1 means `double`.

* find all objects where address is object
```
db.crew.find( {address: {$type: "object"}} ).pretty();
db.crew.find( {address: {$type: 3}} ).pretty();
```

* find all objects where address is string
```
db.crew.find( {address: {$type: "string"}} ).pretty();
db.crew.find( {address: {$type: 2}} ).pretty();
```

* find all objects where address is null type (it will return only objects where `address` field exist and is null type)
```
 db.crew.find( {address: {$type: null}} ).pretty();
 db.crew.find( {address: {$type: 10}} ).pretty();
```

#### specifying read concerns
```
db.movies.find( {runtime: {$eq: 11}}, {runtime:1, title:1, _id:0} ).pretty().limit(3).sort({title: -1}).readConcern("majority")
```

```
db.movies.find( {runtime: {$eq: 11}}, {runtime:1, title:1, _id:0} ).pretty().limit(3).sort({title: -1}).readConcern("linearizable").maxTimeMS(10000)
```

>NOTE: in case very busy system when we run both above queries the second query might return different data then the first query because `linearizable` only returns the data after all the previous write operations commit data into all replicas. QUESTION: really all replicas or majority? [Docs](https://docs.mongodb.com/manual/reference/read-concern-linearizable/#mongodb-readconcern-readconcern.-linearizable-) says that it is 'https://docs.mongodb.com/manual/reference/read-concern-linearizable/#mongodb-readconcern-readconcern.-linearizable-'. Also docs says: "  Linearizable read concern guarantees only apply if read operations specify a query filter that uniquely identifies a single document."

#### free text search

* text index
  * support fast text searches on string and arrays of string fields
  * you cannot perform free text searches without a text index


* first create text index for fields `name` and `skills`.
```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.crew.createIndex({name: "text", skills: "text"})
name_text_skills_text
```

* next run the search

It looks that text search is not case sensitive and it works SOMETIMES with "starts with" substrings. TBD: what are the rules? Maybe the search text has to be a full world and then the results might be returned.

```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.crew.find({ $text: { $search: "Engineering anna"} })
[
  {
    _id: ObjectId("61014b1c5b94f0bdbef0accb"),
    name: 'Anna Smith',
    skills: [ 'technical', 'management' ],
    address: { city: 'Bucharest', country: 'Romania' }
  },
  {
    _id: ObjectId("61014b1c5b94f0bdbef0accd"),
    name: 'Gunter Hoff',
    skills: [ 'engineering' ],
    address: { city: 'Berlin', country: 'Germany' }
  }
]
```

It is not clear why `Anna Smith` is not returned by this query:
```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.crew.find({ $text: { $search: "Engineer Ann"} })
[
  {
    _id: ObjectId("61014b1c5b94f0bdbef0accd"),
    name: 'Gunter Hoff',
    skills: [ 'engineering' ],
    address: { city: 'Berlin', country: 'Germany' }
  },
```
but this query will return no documents at all (maybe because `Enginee` is not an English word???):
```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.crew.find({ $text: { $search: "Enginee Ann"} })

```

##### full text search relevance

You can aggregate results by score using the `$meta` projection operator. Next you can sort by the score to have most relevant results on the top.

```
db.crew.find({ $text: { $search: "Engineering anna"} }, {score: {$meta: "textScore"}})
[
 {
    _id: ObjectId("61014b1c5b94f0bdbef0accd"),
    name: 'Gunter Hoff',
    skills: [ 'engineering' ],
    address: { city: 'Berlin', country: 'Germany' },
    score: 1
  },
  {
    _id: ObjectId("61014b1c5b94f0bdbef0accb"),
    name: 'Anna Smith',
    skills: [ 'technical', 'management' ],
    address: { city: 'Bucharest', country: 'Romania' },
    score: 0.75
  }  
]
```

```
db.crew.find({ $text: { $search: "Engineering anna"} }, {score: {$meta: "textScore"}}).sort({score: {$meta: "textScore"}})
```

#### Sample queries

>NOTE : before running queries in `mongosh` run `use` command to switch to selected database because `find` operation does not return an error 
if it is run agains not existing collection, for example it will return count 0.

```

db.zips.find( { "pop" : { "$lt": 1000 } } ).count()

db.inspections.find( { "result" : "Out of Business", "sector": "Home Improvement Contractor - 100" } ).count()

db.inspections.find( { "$or": [ { "date": "Feb 20 2015" }, { "date": "Feb 21 2015" } ], "sector": { "$ne": "Cigarette Retail Dealer - 127" }}).pretty()

db.zips.find( { "pop" : { "$lt": 1000000, "$gt": 5000 } } ).count()
```

```expr``` can be used to compare fields from the same document (if we use `expr` we have to use `$` sign before field names). `expr` uses **aggregation syntax**,
it does not use MQL syntax:
```
{"$expr": {"eq": ["$start station id", "$end station id"]}}
db.trips.find({"$expr": {"$eq": ["$start station id", "$end station id"]}})
db.trips.find({"$expr": {"$eq": ["$start station id", "$end station id"]}}).count()
db.trips.find({"$expr": {"$and": [ { "$gt": ["$tripduration", 1200] }, { "$eq": ["$start station id", "$end station id"] } ]}}).count()
db.companies.find({"$expr": {"$eq": ["$permalink", "$twitter_username"]}}).count()
```

> NOTE: be careful if you do a typo, for example in the query will be `"$gt": ["tripduration", 1200] }` instead of `"$gt": ["$tripduration", 1200] }` this filter 
> will be evaluated as `true` value wihout throwing any arror.

### Cursor

Cursor is a virtual object where MongoDB stores the documents returned by the find method.

Some of cursor methods:

```
db.collection.find().pretty()
db.collection.find().limit(5)
db.collection.find().skip(3)
db.collection.find().sort({...})
db.collection.find().count()
```

Combinations are also allowed:

```
db.collection.find().skip(2).limit(2)
```

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

### Updating multiple records in C-Sharp

>NOTE: this section is coded using integration tests. To run this test you have to select [.runsettings](/AspNetCoreWebApiMongoDB/AspNetCoreWebApiMongoDB.IntegrationTests/.runsettings) file.


![041_vs_test_runsettings.png](images/041_vs_test_runsettings.png)


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

# Relational SQL vs Document DB

![039_sql_vs_documentdb.png](images/039_sql_vs_documentdb.png)

![040_sql_vs_documentdb.png](images/040_sql_vs_documentdb.png)

# Common SQL concepts and semantics to MongoDB

## Create table/collection
```
CREATE TABLE user (
id MEDIUMINT NOT NULL 
AUTO_INCREMENT,
name varchar(50),
age int,
PRIMARY KEY (id)
)
```
```
db.createCollection(“user")
db.user.insertOne({
name: “John Smith”, 
age: 42
})
```

## Add columns/fields
```
ALTER TABLE user
ADD email varchar(100)
```
```
db.user.updateMany(
{ },
{ $set: { email: '' } }
)
```

## Create index
```
CREATE INDEX
idx_user_name_age
ON user(name, age DESC)
```
```
db.people.createIndex( 
{ name: 1, age: -1 } 
)
```

## Insert statement
```
NSERT INTO user (name, age, 
email)
VALUES (‘Roger’, 46, 
‘roger@email.com’)
```
```
db.user.insertOne(
{
name: “Roger”, 
age: 46,
email: “roger@email.com” 
}
)
```
## Select statement
```
SELECT * FROM user
```
```
db.user.find()
db.user.find( {} )
```

## Select statement - filter
```
ELECT name, age
FROM user
WHERE age > 20
```
```
db.user.find(
{ age: { $gt: 20 } },
{name: 1, age: 1, _id: 0 }
)
```
```
SELECT name, age
FROM user
WHERE age > 20
LIMIT 5
SKIP 10
```
```
SELECT name, age
FROM user
WHERE age > 20
LIMIT 5
SKIP 10
```

## Update statement
```
UPDATE user
SET email = ‘NA’
WHERE age < 18
```
```
db.user.updateMany(
{ age: { $lt: 18 } },
{ $set: {email : “NA" } }
)
```
## Delete statement
```
DELETE FROM user
WHERE age < 18
```
```
db.user.deleteMany(
{ age: { $lt: 18 } })
```
Delete all:
```
DELETE FROM user
```
```
db.user.deleteMany( {} 
```

## Drop table/collection
```
DROP TABLE user
```
```
db.user.drop()
```

# Importing.exporting data

Install [Database Tools](https://docs.mongodb.com/database-tools/installation/installation-windows/).


## Importing

Next run (it will create the database if it does not exist):

```
PS D:\GitHub\kicaj29\MongoDB\data\sampledb> mongoimport --uri "mongodb+srv://kicaj:kicaj@myfirstcluster.a6uds.mongodb.net" --file aircraft.json --collection aircraft --db flightmgmt --drop  
2021-07-27T14:38:13.190+0200    connected to: mongodb+srv://[**REDACTED**]@myfirstcluster.a6uds.mongodb.net
2021-07-27T14:38:13.219+0200    dropping: flightmgmt.aircraft
2021-07-27T14:38:13.309+0200    9 document(s) imported successfully. 0 document(s) failed to import.
```

and second collection:
```
PS D:\GitHub\kicaj29\MongoDB\data\sampledb> mongoimport --uri "mongodb+srv://kicaj:kicaj@myfirstcluster.a6uds.mongodb.net" --file flights.json --collection flights --db flightmgmt --drop
2021-07-27T14:40:59.649+0200    connected to: mongodb+srv://[**REDACTED**]@myfirstcluster.a6uds.mongodb.net
2021-07-27T14:40:59.679+0200    dropping: flightmgmt.flights
2021-07-27T14:40:59.771+0200    10 document(s) imported successfully. 0 document(s) failed to import.
```

and next
```
PS D:\GitHub\kicaj29\MongoDB\data\sampledb> mongoimport --uri "mongodb+srv://kicaj:kicaj@myfirstcluster.a6uds.mongodb.net" --file crew.json --collection crew --db flightmgmt --drop
2021-07-27T14:44:21.094+0200    connected to: mongodb+srv://[**REDACTED**]@myfirstcluster.a6uds.mongodb.net
2021-07-27T14:44:21.125+0200    dropping: flightmgmt.crew
2021-07-27T14:44:21.217+0200    4 document(s) imported successfully. 0 document(s) failed to import.
```

>NOTE: if we deal with BSON format then use command `mongorestore` to do the import. Example: `mongoimport --uri "mongodb+srv://kicaj:kicaj@myfirstcluster.a6uds.mongodb.net" --drop  dump` (dump is a folder name with dumped data).

## Exporting

There are 2 commands:

* for JSON: `mongoexport`, sample run:
  ```cmd
  PS D:\Programs\mongodb-database-tools-windows-x86_64-100.5.0\mongodb-database-tools-windows-x86_64-100.5.0\bin> .\mongoexport.exe "mongodb+srv://m001-student:[PASS]]@sandbox.[NAME].mongodb.net/sample_analytics" --collection="accounts" --out=accounts.json
  2022-03-17T12:40:21.574+0100    connected to: mongodb+srv://[**REDACTED**]@sandbox.mjboj.mongodb.net/sample_analytics
  2022-03-17T12:40:21.849+0100    exported 1746 records
  ```
* for BSON: `mongodump`, sample run:
  ```cmd
  PS D:\Programs\mongodb-database-tools-windows-x86_64-100.5.0\mongodb-database-tools-windows-x86_64-100.5.0\bin> .\mongodump.exe "mongodb+srv://m001-student:[PASS]@sandbox.[NAME].mongodb.net/sample_analytics"
  2022-03-17T12:37:05.930+0100    WARNING: On some systems, a password provided directly in a connection string or using --uri may be visible to system status programs such as `ps` that may be invoked by other users. Consider omitting the password to provide it via stdin, or using the --config option to specify a configuration file with the password.
  2022-03-17T12:37:07.105+0100    writing sample_analytics.accounts to dump\sample_analytics\accounts.bson
  2022-03-17T12:37:07.214+0100    writing sample_analytics.customers to dump\sample_analytics\customers.bson
  2022-03-17T12:37:07.215+0100    writing sample_analytics.transactions to dump\sample_analytics\transactions.bson
  2022-03-17T12:37:07.266+0100    done dumping sample_analytics.accounts (1746 documents)
  2022-03-17T12:37:07.368+0100    done dumping sample_analytics.customers (500 documents)
  2022-03-17T12:37:08.663+0100    done dumping sample_analytics.transactions (1746 documents)
  ```

# References/docs
https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-5.0&tabs=visual-studio   

# Oplog

Oplog is stored in `local` database. It is stored in primary and secondary nodes (here through replication).

To read oplog entry run for example:


```js
Atlas atlas-mritki-shard-0 [primary] local> db.oplog.rs.findOne()
{
  lsid: {
    id: UUID("174bef69-f4e0-44cd-9435-16349814a5fb"),
    uid: Binary(Buffer.from("75daa8b7af2f2e90543e73aea858f671d0a5bff76ffef17d6554b092760712c1", "hex"), 0)
  },
  txnNumber: Long("1"),
  op: 'i',
  ns: 'flightmgmt.crew',
  ui: UUID("a9e12c39-5e11-4a47-939e-5abb8486a441"),
  o: {
    _id: ObjectId("61113ba8b85f0697736d0077"),
    name: 'MondayCrew',
    skills: [],
    address: { city: null, country: null }
  },
  ts: Timestamp(6, 1628519337),
  t: Long("130"),
  v: Long("2"),
  wall: ISODate("2021-08-09T14:28:57.054Z"),
  stmtId: 0,
  prevOpTime: { ts: Timestamp(0, 0), t: Long("-1") }
}
```
* `op` value is type of operation: i (insert), u (update), d (delete), n (no operation)
* `ns` means namespace

In case we create a collection:

```
Atlas atlas-mritki-shard-0 [primary] flightmgmt> db.mytbl.insert({"name": "test", age: 10})
DeprecationWarning: Collection.insert() is deprecated. Use insertOne, insertMany, or bulkWrite.
{
  acknowledged: true,
  insertedIds: { '0': ObjectId("6112cb1725a995e781f0cdb4") }
}
```

next we can find oplog entry for this operation:

```js
Atlas atlas-mritki-shard-0 [primary] local> db.oplog.rs.find({"o.create": "mytbl"})
[
  {
    op: 'c',
    ns: 'flightmgmt.$cmd',
    ui: UUID("12707f7e-d372-45ad-80aa-c1aa941b5066"),
    o: {
      create: 'mytbl',
      idIndex: { v: 2, key: { _id: 1 }, name: '_id_' }
    },
    ts: Timestamp(11, 1628621591),
    t: Long("131"),
    v: Long("2"),
    wall: ISODate("2021-08-10T18:53:11.097Z")
  }
]
```

>NOTE: it looks that in oplog we do not store `_id` value!

## Oplog stats

```js
Atlas atlas-mritki-shard-0 [primary] local> db.oplog.rs.stats()
{
  ns: 'local.oplog.rs',
  size: 8174213871,
  count: 11520420,
  avgObjSize: 709,
  storageSize: 2541527040,
  freeStorageSize: 1045405696,
  capped: true,
  max: -1,
  maxSize: Long("8246187008"),
  sleepCount: 0,
  sleepMS: 0,
  nindexes: 0,
  indexBuilds: [],
  totalIndexSize: 0,
  totalSize: 2541527040,
  indexSizes: {},
  scaleFactor: 1,
  ok: 1,
  '$clusterTime': {
    clusterTime: Timestamp(126, 1628622879),
    signature: {
      hash: Binary(Buffer.from("f1bbe9d4c7092668d2a3b344a836fbd4d1855716", "hex"), 0),
      keyId: Long("6961452270902837249")
    }
  },
  operationTime: Timestamp(126, 1628622879)
}
```

## replication info

```js
Atlas atlas-mritki-shard-0 [primary] local> rs.printReplicationInfo()
configured oplog size
'8179550531 MB'
---
log length start to end
'102254 secs (28.4 hrs)'
---
oplog first event time
'Mon Aug 09 2021 16:28:57 GMT+0200 (czas środkowoeuropejski letni)'
---
oplog last event time
'Tue Aug 10 2021 20:53:11 GMT+0200 (czas środkowoeuropejski letni)'
---
now
'Tue Aug 10 2021 21:16:16 GMT+0200 (czas środkowoeuropejski letni)'
```

## replication info on secondary nodes

>"Prints a formatted report of the replica set status from the perspective of the secondary member of the set. The output is identical to db.printSecondaryReplicationInfo()."

[Docs](https://docs.mongodb.com/manual/reference/method/rs.printSecondaryReplicationInfo/).

It did not work:

```js
Atlas atlas-mritki-shard-0 [primary] local> rs.printSecondaryReplicationInfo()
MongoServerError: namespace not found
Atlas atlas-mritki-shard-0 [primary] local> db.printSecondaryReplicationInfo()
MongoServerError: namespace not found
```

## replication service status

```js
Atlas atlas-mritki-shard-0 [primary] local> db.serverStatus().repl
{
  topologyVersion: {
    processId: ObjectId("6112a55c923882e9a084c983"),
    counter: Long("6")
  },
  hosts: [
    'myfirstcluster-shard-00-00.a6uds.mongodb.net:27017',
    'myfirstcluster-shard-00-01.a6uds.mongodb.net:27017',
    'myfirstcluster-shard-00-02.a6uds.mongodb.net:27017'
  ],
  setName: 'atlas-mritki-shard-0',
  setVersion: 7,
  ismaster: true,
  secondary: false,
  primary: 'myfirstcluster-shard-00-01.a6uds.mongodb.net:27017',
  tags: {
    provider: 'AWS',
    nodeType: 'ELECTABLE',
    region: 'EU_CENTRAL_1',
    workloadType: 'OPERATIONAL'
  },
  me: 'myfirstcluster-shard-00-01.a6uds.mongodb.net:27017',
  electionId: ObjectId("7fffffff0000000000000083"),
  lastWrite: {
    opTime: { ts: Timestamp(26, 1628623553), t: Long("131") },
    lastWriteDate: ISODate("2021-08-10T19:25:53.000Z"),
    majorityOpTime: { ts: Timestamp(26, 1628623553), t: Long("131") },
    majorityWriteDate: ISODate("2021-08-10T19:25:53.000Z")
  },
  rbid: 1
}
```

# Backup and restore

* Backup
In case of local mongodb database just run `mongodump` and it will create `dump` folder in the current folder.
By default it will connect to local mongo db that works on url: `mongodb://localhost:27017`.

* Restore
To restore db in local instance of mongodb that runs on default port run: `mongorestore dump`. The `dump` folder is the same folder that was created by `mongodump`. NOTE: this folder contains also `admin` folder and it looks that this folder is mandatory to execute restore because when I moved this folder to antoher location then the dum did not work.


# Aggregations

https://www.mongodb.com/docs/manual/reference/operator/aggregation/group/
```
db.listingsAndReviews.aggregate([
  {
    $project: { "address": 1, "_id": 0}
  },
  {
    $group: { "_id": "$address.country" }
  }
])
```
```
db.listingsAndReviews.aggregate([
  {
    $project: { "address": 1, "_id": 0}
  },
  {
    $group: 
      { 
        "_id": "$address.country",
        "count": { "$sum": 1 }
      }
  }
])
```
```
db.listingsAndReviews.aggregate([
  {
    $project: { "room_type": 1, "_id": 0}
  },
  {
    $group: { "_id": "$room_type" }
  }
])
```



# AspNetCore and MongoDB

[SourceCode](/AspNetCoreWebApiMongoDB)


