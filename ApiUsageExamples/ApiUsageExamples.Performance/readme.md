# Scenario 1 - additional lists are empty

https://www.mongodb.com/docs/manual/reference/command/collStats/#output

Selected stats descriptions:

* `size`: total uncompressed size in memory of all records in a collection, it does not include any indexes associated with the collection, which the 
`totalIndexSize` field reports
* `storageSize`: the total amount of storage allocated to this collection for document storage. If collection data is compressed (which is the default for `WiredTiger`), the storage size **reflects the compressed size** and **may be smaller than the value for `size`**. `storageSize`
 does not include index size (see `totalIndexSize`).
 for index sizing.
* `totalIndexSize`: the total size of all indexes. **If an index uses prefix compression** (which is the default for `WiredTiger`), the returned size **reflects the compressed size** for any such indexes when calculating the total.
* `totalSize`: the sum of the `storageSize` and `totalIndexSize`


https://www.mongodb.com/docs/manual/tutorial/ensure-indexes-fit-ram/

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

## How to collect stats for `Persons_ClusteredCollection`:

* `db.Persons_NonClusteredCollection.stats().size`
* `db.Persons_NonClusteredCollection.stats().storageSize`
* `db.Persons_NonClusteredCollection.stats().indexSizes`
* `db.Persons_NonClusteredCollection.stats().totalIndexSize`
* `db.Persons_NonClusteredCollection.stats().totalSize`
* `db.Persons_NonClusteredCollection.stats().wiredTiger.cache`
* `db.Persons_NonClusteredCollection.stats({ indexDetails: true }).indexDetails._id_.cache`

## Server configuration

`db.serverStatus().wiredTiger.cache` returns `'maximum bytes configured': 268 435 456` it is **max cache size**.


| Tables                          |      size      |  storageSize | indexSizes        |totalIndexSize | totalSize | wiredTiger.cache                           | indexDetails._id_.cache                     |
|---------------------------------|---------------:|-------------:|------------------:|--------------:|-----------|-------------------------------------------:|--------------------------------------------:|
| Persons_ClusteredCollection     |  1 277 788     | 221 184      |           N/A     |             0 | 221 184   |'bytes currently in the cache': 2 503 302   |  N/A                                        |       
| Persons_NonClusteredCollection  |  1 277 788     | 208 896      | { _id_: 114 688 } |  114 688      | 323 584   |'bytes currently in the cache': 2 384 911   | 'bytes currently in the cache': 1 131 452   |


## No extra data

|Test number|Clustered collection|Non clustered collection|
|----------:|-------------------:|-----------------------:|
|1          |00:00:17.55         |00:00:18.29             |
|2          |00:00:16.09         |00:00:17.85             |
|3          |00:00:15.51         |00:00:16.88             |
|4          |00:00:15.87         |00:00:16.74             |
|5          |00:00:16.83         |00:00:19.25             |


