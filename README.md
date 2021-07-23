- [Basics](#basics)
  - [MongoDB is document database type.](#mongodb-is-document-database-type)
  - [CAP model for MongoDB](#cap-model-for-mongodb)
  - [SQL Terms vs Mongo DB Terms](#sql-terms-vs-mongo-db-terms)
  - [JSON vs BSON](#json-vs-bson)
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