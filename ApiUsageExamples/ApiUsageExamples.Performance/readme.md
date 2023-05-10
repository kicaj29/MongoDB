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


How to collect stats for `Persons_ClusteredCollection`:

* db.Persons_ClusteredCollection.stats().size
* db.Persons_ClusteredCollection.stats().storageSize
* db.Persons_ClusteredCollection.stats().totalIndexSize
* db.Persons_ClusteredCollection.stats().totalSize
* db.Persons_ClusteredCollection.stats().indexSizes
* db.Persons_ClusteredCollection.stats().wiredTiger.cache
* db.Persons_NonClusteredCollection.stats({ indexDetails: true }).indexDetails

How to collect stats for `Persons_ClusteredCollection`:

* db.Persons_NonClusteredCollection.stats().size
* db.Persons_NonClusteredCollection.stats().storageSize
* db.Persons_NonClusteredCollection.stats().totalIndexSize
* db.Persons_NonClusteredCollection.stats().indexSizes
* db.Persons_NonClusteredCollection.stats().wiredTiger.cache
* db.Persons_NonClusteredCollection.stats({ indexDetails: true }).indexDetails._id_.cache

Results:

| Tables                          |      size      |  storageSize | indexSizes       |totalIndexSize | totalSize | wiredTiger.cache                           | indexDetails._id_.cache                     |
|---------------------------------|---------------:|-------------:|-----------------:|--------------:|-----------|-------------------------------------------:|--------------------------------------------:|
| Persons_ClusteredCollection     |  1 277 788     | 221 184      |           N/A    |             0 | 221 184   |'bytes currently in the cache': 2 503 838   |  N/A                                        |       
| Persons_NonClusteredCollection  |  1 277 788     | 208 896      | { _id_: 114688 } |               | 114 688   | 323 584   |'bytes currently in the cache': 2 384 850   | 'bytes currently in the cache': 1 131 452   |

Run 01
Read from clustered collection: 00:00:17.2014820.
Read from non clustered collection: 00:00:18.2659679.

