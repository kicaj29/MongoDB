# Scenario 1 - Clustered collection vs non clustered collection

https://www.mongodb.com/docs/manual/reference/command/collStats/#output

Selected stats descriptions:

* `size`: total uncompressed size in memory of all records in a collection, it does not include any indexes associated with the collection, which the 
`totalIndexSize` field reports
* `storageSize`: the total amount of storage allocated to this collection for document storage. If collection data is compressed (which is the default for `WiredTiger`), the storage size **reflects the compressed size** and **may be smaller than the value for `size`**. `storageSize`
 does not include index size (see `totalIndexSize`).
 for index sizing.
* `totalIndexSize`: the total size of all indexes. **If an index uses prefix compression** (which is the default for `WiredTiger`), the returned size **reflects the compressed size** for any such indexes when calculating the total.
* `totalSize`: the sum of the `storageSize` and `totalIndexSize`


https://www.mongodb.com/docs/manual/tutorial/ensure-indexes-fit-ram/ - it is import to know if the indexes are cached

https://www.mongodb.com/docs/manual/core/clustered-collections/#behavior
*"The clustered index keys are stored with the collection. The collection size returned by the collStats command includes the clustered index size."*

## How to collect stats for `Persons_ClusteredCollection`:

* `db.Persons_ClusteredCollection.stats().size`
* `db.Persons_ClusteredCollection.stats().storageSize`
* `db.Persons_ClusteredCollection.stats().indexSizes`
* `db.Persons_ClusteredCollection.stats().totalIndexSize`
* `db.Persons_ClusteredCollection.stats().totalSize`
* `db.Persons_ClusteredCollection.stats().wiredTiger.cache`
* `db.Persons_ClusteredCollection.stats({ indexDetails: true }).indexDetails`

## How to collect stats for `Persons_NonClusteredCollection`:

* `db.Persons_NonClusteredCollection.stats().size`
* `db.Persons_NonClusteredCollection.stats().storageSize`
* `db.Persons_NonClusteredCollection.stats().indexSizes`
* `db.Persons_NonClusteredCollection.stats().totalIndexSize`
* `db.Persons_NonClusteredCollection.stats().totalSize`
* `db.Persons_NonClusteredCollection.stats().wiredTiger.cache`
* `db.Persons_NonClusteredCollection.stats({ indexDetails: true }).indexDetails._id_.cache`

## Server configuration

`db.serverStatus().wiredTiger.cache` returns `'maximum bytes configured': 268 435 456` it is **max cache size**.

## No extra data - 10k documents, 10k queries

* Stats

| Tables                          |      size      |  storageSize | indexSizes        |totalIndexSize | totalSize | wiredTiger.cache                           | indexDetails._id_.cache                     |
|---------------------------------|---------------:|-------------:|------------------:|--------------:|-----------|-------------------------------------------:|--------------------------------------------:|
| Persons_ClusteredCollection     |  1 277 788     | 221 184      |           N/A     |             0 | 221 184   |'bytes currently in the cache': 2 503 302   |  N/A                                        |       
| Persons_NonClusteredCollection  |  1 277 788     | 208 896      | { _id_: 114 688 } |  114 688      | 323 584   |'bytes currently in the cache': 2 384 911   | 'bytes currently in the cache': 1 131 452   |


* Reading data

|Test number|Clustered collection [s]|Non clustered collection [s]|
|----------:|-----------------------:|---------------------------:|
|1          |00:00:17.55             |00:00:18.29                 |
|2          |00:00:16.09             |00:00:17.85                 |
|3          |00:00:15.51             |00:00:16.88                 |
|4          |00:00:15.87             |00:00:16.74                 |
|5          |00:00:16.83             |00:00:19.25                 |
|6          |00:00:15.84             |00:00:18.12                 |
|7          |00:00:19.40             |00:00:18.55                 |


## With extra data - 10k documents, 50 items in every list, 10k queries

* Stats

| Tables                          |      size      |  storageSize | indexSizes        |totalIndexSize | totalSize   | wiredTiger.cache                            | indexDetails._id_.cache                     |
|---------------------------------|---------------:|-------------:|------------------:|--------------:|-------------|--------------------------------------------:|--------------------------------------------:|
| Persons_ClusteredCollection     | 32 517 788     | 2 306 048    |           N/A     |             0 | 2 306 048   |'bytes currently in the cache': 35 454 915   |  N/A                                        |       
| Persons_NonClusteredCollection  | 32 517 788     | 2 289 664    | { _id_: 114 688 } |  114 688      | 2 404 352   |'bytes currently in the cache': 35 430 435   | 'bytes currently in the cache': 270 598     |


* Reading data]

|Test number|Clustered collection [s]|Non clustered collection [s]|
|----------:|-----------------------:|---------------------------:|
|1          |00:00:16.18             |00:00:18.65                 |
|2          |00:00:12.90             |00:00:15.47                 |
|3          |00:00:14.02             |00:00:16.88                 |
|4          |00:00:16.80             |00:00:18.19                 |
|5          |00:00:16.58             |00:00:16.99                 |
|6          |00:00:19.27             |00:00:18.66                 |
|7          |00:00:16.99             |00:00:18.02                 |


## With extra data - 30k documents, 50 items in every list, 30k queries

* Stats

| Tables                          |      size      |  storageSize | indexSizes        |totalIndexSize | totalSize   | wiredTiger.cache                            | indexDetails._id_.cache                     |
|---------------------------------|---------------:|-------------:|------------------:|--------------:|-------------|--------------------------------------------:|--------------------------------------------:|
| Persons_ClusteredCollection     | 97 597 788     | 6 881 280    |           N/A     |             0 | 6 881 280   |'bytes currently in the cache': 106 410 861  |  N/A                                        |       
| Persons_NonClusteredCollection  | 97 597 788     | 6 856 704    | { _id_: 307 200 } |  307 200      | 71 63 904   |'bytes currently in the cache': 106 067 989  | 'bytes currently in the cache': 826 251     |


* Reading data

|Test number|Clustered collection [s]|Non clustered collection [s]|
|----------:|-----------------------:|---------------------------:|
|1          |00:00:55.76             |00:00:59.42                 |
|2          |00:00:47.47             |00:00:49.86                 |
|3          |00:00:48.04             |00:00:50.61                 |

## Summary

* Before running tests make sure that all indexes are cached: https://www.mongodb.com/docs/manual/tutorial/ensure-indexes-fit-ram/
* Query using random order of ids: https://www.mongodb.com/docs/manual/tutorial/ensure-indexes-fit-ram/#indexes-that-hold-only-recent-values-in-ram
* Clustered collections are faster but only a little bit
  * It is harder to code creating clustered collections because they have to be created explicitly and if the collection already exist it throws exception: `MongoCommandException: Command create failed: Collection PerformanceTests.Persons_ClusteredCollection already exists..`. To manage it dedicated service could be used to provision necessary collections in MongoDB before worker service will start using it.