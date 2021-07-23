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

* Majority: this is the default for all the fix operators if you do not specify and of the read concerns. In this case the query returns a data that has been acknowledged by the majority of the replica set members. The documents returned by the read operation are durable, even in the event of failure.

* Linearisible: the query returns data that reflects all successful majority-acknowledged writes that completed prior to the start of the read operation. The query may wait for concurrently executing writes to propagate to a majority of replica set members before returning results.

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

