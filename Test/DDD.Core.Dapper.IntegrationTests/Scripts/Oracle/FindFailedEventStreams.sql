BEGIN
  SPCLEARSCHEMA();
END;
/
Insert into TEST.FAILEDEVENTSTREAM (STREAMID,STREAMTYPE,STREAMSOURCE,STREAMPOSITION,EVENTID,EVENTTYPE,EXCEPTIONTIME,EXCEPTIONTYPE,EXCEPTIONMESSAGE,EXCEPTIONSOURCE,EXCEPTIONINFO,BASEEXCEPTIONTYPE,BASEEXCEPTIONMESSAGE,RETRYCOUNT,RETRYMAX,RETRYDELAYS,BLOCKSIZE) values ('2','MessageBox','COL','7A70770A47C11B9E883A08DA0E368663','4DDD0AE15118DE7E883B08DA0E368663','DDD.Collaboration.Domain.Messages.MessageBoxCreated, DDD.Collaboration.Messages',to_date('19/11/21','DD/MM/RR'),'System.Exception, mscorlib','Invalid event','DDD.IdentityManagement, DDD.IdentityManagement.Application.MessageBoxCreatedEventHandler, Void Handle()','System.Exception: Invalid event ---> System.Exception: Format not supported.
   --- End of inner exception stack trace ---','System.Exception, mscorlib','Format not supported.','0','5','10,60,360','100')
/