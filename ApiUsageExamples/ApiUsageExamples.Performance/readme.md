# Scenario 1 - additional lists are empty

https://www.mongodb.com/docs/manual/reference/command/collStats/#output

Selected stats descriptions:

* `size`: total uncompressed size in memory of all records in a collection, it does not include any indexes associated with the collection, which the 
`totalIndexSize` field reports
* `storageSize`: the total amount of storage allocated to this collection for document storage. If collection data is compressed (which is the default for `WiredTiger`), the storage size **reflects the compressed size** and **may be smaller than the value for `size`**. `storageSize`
 does not include index size (see `totalIndexSize`).
 for index sizing.
* `totalIndexSize`: the total size of all indexes. If an index uses prefix compression (which is the default for `WiredTiger`), the returned size reflects the compressed size for any such indexes when calculating the total.
* `totalSize`: the sum of the `storageSize` and `totalIndexSize`

Run 01
Read from clustered collection: 00:00:17.2014820.
Read from non clustered collection: 00:00:18.2659679.

* Clustered collection
https://www.mongodb.com/docs/manual/core/clustered-collections/#behavior
*"The clustered index keys are stored with the collection. The collection size returned by the collStats command includes the clustered index size."*

db.Persons_ClusteredCollection.stats().size                          1 277 788      (total uncompressed size in memory of all records in a collection, it does not include any indexes associated with the collection)
db.Persons_ClusteredCollection.stats().totalSize                       221 184      (the sum of the storageSize and totalIndexSize)
db.Persons_ClusteredCollection.stats().storageSize                     221 184      (total amount of storage allocated to this collection for document storage)
db.Persons_ClusteredCollection.stats().totalIndexSize                        0
db.Persons_ClusteredCollection.stats().indexSizes                            { }

db.Persons_ClusteredCollection.stats().wiredTiger.cache
*'bytes currently in the cache': 2 503 838*

db.Persons_NonClusteredCollection.stats({ indexDetails: true }).indexDetails
No index details available

* None clustered collection

db.Persons_NonClusteredCollection.stats().size                        1 277 788
db.Persons_NonClusteredCollection.stats().totalSize                     323 584     (208 896 + 114 688)
db.Persons_NonClusteredCollection.stats().storageSize                   208 896
db.Persons_NonClusteredCollection.stats().totalIndexSize                114 688
db.Persons_NonClusteredCollection.stats().indexSizes            { _id_: 114 688 }

db.Persons_NonClusteredCollection.stats().wiredTiger.cache
*'bytes currently in the cache': 2 384 850'*

db.Persons_NonClusteredCollection.stats({ indexDetails: true }).indexDetails._id_.cache
*'bytes currently in the cache': 1 131 452'*