# LockTracer


My goal is to detect deadlocks before they happen, e.g. if you have two resources, you know you have to always use them in the same order, otherwise a deadlock might occur.


      lock (lockObj1) 
      lock (lockObj2) 
      { 
          // some code
      } 
      

  
... somewhere else in the app ...


      lock (lockObj2) 
      lock (lockObj1) // <- I expect some "possible deadlock" detection here 
      { 
          // some code
      } 
      

  
In this case I'm using lockObj1 then lockObj2 in one place and using them in the opposite order in another place, this is something you will like to avoid in an application.
Of course, lock statements don't need to be used one after the other like in the example, your complex application might have several complex objects interacting with each other

