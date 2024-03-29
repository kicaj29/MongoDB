- [Docs](#docs)
- [Server parameters](#server-parameters)
  - [Cache size](#cache-size)


# Docs

https://www.mongodb.com/docs/manual/reference/command/collStats/#output

# Server parameters

* Connect to the server: `PS D:\Programs\mongosh-1.8.2-win32-x64\mongosh-1.8.2-win32-x64\bin> .\mongosh.exe "mongodb://localhost:27017/?readPreference=primary&directConnection=true&ssl=false"`

* Next run `db.serverStatus()` to see all configuration.

## Cache size

```
test> db.serverStatus().wiredTiger.cache                                                     
{                                                                                            
  'application threads page read from disk to cache count': 4,                               
  'application threads page read from disk to cache time (usecs)': 1058,                     
  'application threads page write from cache to disk count': 4365,                           
  'application threads page write from cache to disk time (usecs)': 4058733,                 
  'bytes allocated for updates': 204303,                                                     
  'bytes belonging to page images in the cache': 6027469,                                    
  'bytes belonging to the history store table in the cache': 191,                            
  /* CURRENT CACHE SIZE */ 'bytes currently in the cache': 6760601,                                            
  'bytes dirty in the cache cumulative': 129510025,                                          
  'bytes not belonging to page images in the cache': 733132,                                 
  'bytes read into cache': 9112,                                                             
  'bytes written from cache': 73878373,                                                      
  'checkpoint blocked page eviction': 0,                                                     
  'checkpoint of history store file blocked non-history store page eviction': 0,             
  'eviction calls to get a page': 359,                                                       
  'eviction calls to get a page found queue empty': 296,                                     
  'eviction calls to get a page found queue empty after locking': 2,                         
  'eviction currently operating in aggressive mode': 0,                                      
  'eviction empty score': 0,                                                                 
  'eviction gave up due to detecting an out of order on disk value behind the last update on 
the chain': 0,                                                                               
  'eviction gave up due to detecting an out of order tombstone ahead of the selected on disk 
update': 0,                                                                                  
  'eviction gave up due to detecting an out of order tombstone ahead of the selected on disk 
update after validating the update chain': 0,                                                
  'eviction gave up due to detecting out of order timestamps on the update chain after the se
lected on disk update': 0,                                                                   
  'eviction gave up due to needing to remove a record from the history store but checkpoint i
s running': 0,                                                                               
  'eviction passes of a file': 737,                                                          
  'eviction server candidate queue empty when topping up': 97,                               
  'eviction server candidate queue not empty when topping up': 0,                            
  'eviction server evicting pages': 0,                                                       
  'eviction server slept, because we did not make progress with eviction': 2690,             
  'eviction server unable to reach eviction goal': 0,                                        
  'eviction server waiting for a leaf page': 0,                                              
  'eviction state': 64,                                                                      
  'eviction walk most recent sleeps for checkpoint handle gathering': 0,                     
  'eviction walk target pages histogram - 0-9': 684,                                         
  'eviction walk target pages histogram - 10-31': 39,                                        
  'eviction walk target pages histogram - 128 and higher': 0,                                
  'eviction walk target pages histogram - 32-63': 14,                                        
  'eviction walk target pages histogram - 64-128': 0,                                        
  'eviction walk target pages reduced due to history store cache pressure': 0,               
  'eviction walk target strategy both clean and dirty pages': 0,                             
  'eviction walk target strategy only clean pages': 0,                                       
  'eviction walk target strategy only dirty pages': 737,                                     
  'eviction walks abandoned': 0,                                                             
  'eviction walks gave up because they restarted their walk twice': 737,                     
  'eviction walks gave up because they saw too many pages and found no candidates': 0,       
  'eviction walks gave up because they saw too many pages and found too few candidates': 0,  
  'eviction walks reached end of tree': 1474,                                                
  'eviction walks restarted': 0,                                                             
  'eviction walks started from root of tree': 737,                                           
  'eviction walks started from saved location in tree': 0,                                   
  'eviction worker thread active': 4,                                                        
  'eviction worker thread created': 0,                                                       
  'eviction worker thread evicting pages': 56,                                               
  'eviction worker thread removed': 0,                                                       
  'eviction worker thread stable number': 0,                                                 
  'files with active eviction walks': 0,                                                     
  'files with new eviction walks started': 737,                                              
  'force re-tuning of eviction workers once in a while': 0,                                  
  'forced eviction - history store pages failed to evict while session has history store curs
or open': 0,                                                                                 
  'forced eviction - history store pages selected while session has history store cursor open
': 0,                                                                                        
  'forced eviction - history store pages successfully evicted while session has history store
 cursor open': 0,                                                                            
  'forced eviction - pages evicted that were clean count': 0,                                
  'forced eviction - pages evicted that were clean time (usecs)': 0,                         
  'forced eviction - pages evicted that were dirty count': 5,                                
  'forced eviction - pages evicted that were dirty time (usecs)': 5049,                      
  'forced eviction - pages selected because of a large number of updates to a single item': 0
,                                                                                            
  'forced eviction - pages selected because of too many deleted items count': 11,            
  'forced eviction - pages selected count': 9,                                               
  'forced eviction - pages selected unable to be evicted count': 0,                          
  'forced eviction - pages selected unable to be evicted time': 0,                           
  'hazard pointer blocked page eviction': 0,                                                 
  'hazard pointer check calls': 65,                                                          
  'hazard pointer check entries walked': 0,                                                  
  'hazard pointer maximum array length': 0,                                                  
  'history store table insert calls': 0,                                                     
  'history store table insert calls that returned restart': 0,                               
  'history store table max on-disk size': 0,                                                 
  'history store table on-disk size': 4096,                                                  
  'history store table out-of-order resolved updates that lose their durable timestamp': 0,  
  'history store table out-of-order updates that were fixed up by reinserting with the fixed 
timestamp': 0,                                                                               
  'history store table reads': 0,                                                            
  'history store table reads missed': 0,                                                     
  'history store table reads requiring squashed modifies': 0,                                
  'history store table truncation by rollback to stable to remove an unstable update': 0,    
  'history store table truncation by rollback to stable to remove an update': 0,             
  'history store table truncation to remove an update': 0,                                   
  'history store table truncation to remove range of updates due to key being removed from th
e data page during reconciliation': 0,                                                       
  'history store table truncation to remove range of updates due to out-of-order timestamp up
date on data page': 0,                                                                       
  'history store table writes requiring squashed modifies': 0,                               
  'in-memory page passed criteria to be split': 8,                                           
  'in-memory page splits': 4,                                                                
  'internal pages evicted': 0,                                                               
  'internal pages queued for eviction': 0,                                                   
  'internal pages seen by eviction walk': 0,                                                 
  'internal pages seen by eviction walk that are already queued': 0,                         
  'internal pages split during eviction': 0,                                                 
  'leaf pages split during eviction': 6,                                                     
  /* MAX CACHE SIZE  */'maximum bytes configured': 268435456,                                                     
  'maximum page size at eviction': 0,                                                        
  'modified pages evicted': 61,                                                              
  'modified pages evicted by application threads': 0,                                        
  'operations timed out waiting for space in cache': 0,                                      
  'overflow pages read into cache': 0,                                                       
  'page split during eviction deepened the tree': 0,                                         
  'page written requiring history store records': 0,                                         
  'pages currently held in the cache': 103,                                                  
  'pages evicted by application threads': 0,                                                 
  'pages evicted in parallel with checkpoint': 0,                                            
  'pages queued for eviction': 52,                                                           
  'pages queued for eviction post lru sorting': 50,                                          
  'pages queued for urgent eviction': 11,                                                    
  'pages queued for urgent eviction during walk': 0,                                         
  'pages queued for urgent eviction from history store due to high dirty content': 0,        
  'pages read into cache': 4,                                                                
  'pages read into cache after truncate': 57,                                                
  'pages read into cache after truncate in prepare state': 0,                                
  'pages requested from the cache': 147669,                                                  
  'pages seen by eviction walk': 1373,                                                       
  'pages seen by eviction walk that are already queued': 547,                                
  'pages selected for eviction unable to be evicted': 0,                                     
  'pages selected for eviction unable to be evicted because of active children on an internal
 page': 0,                                                                                   
  'pages selected for eviction unable to be evicted because of failure in reconciliation': 0,
  'pages selected for eviction unable to be evicted because of race between checkpoint and ou
t of order timestamps handling': 0,                                                          
  'pages walked for eviction': 213,                                                          
  'pages written from cache': 4450,                                                          
  'pages written requiring in-memory restoration': 15,                                       
  'percentage overhead': 8,                                                                  
  'the number of times full update inserted to history store': 0,                            
  'the number of times reverse modify inserted to history store': 0,                         
  'tracked bytes belonging to internal pages in the cache': 30144,                           
  'tracked bytes belonging to leaf pages in the cache': 6730457,                             
  'tracked dirty bytes in the cache': 0,                                                     
  'tracked dirty pages in the cache': 0,                                                     
  'unmodified pages evicted': 0                                                              
}                                                                                            
```