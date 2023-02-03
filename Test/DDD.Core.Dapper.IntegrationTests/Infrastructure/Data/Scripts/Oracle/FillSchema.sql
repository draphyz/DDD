--------------------------------------------------------
--  File created - Thursday-March-03-2022  
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Procedure SPCLEARSCHEMA
--------------------------------------------------------

CREATE OR REPLACE PROCEDURE TEST.SPCLEARSCHEMA AS 
BEGIN
  FOR c IN (SELECT table_name, constraint_name FROM user_constraints WHERE constraint_type = 'R')
  LOOP
    EXECUTE IMMEDIATE ('alter table ' || c.table_name || ' disable constraint ' || c.constraint_name);
  END LOOP;
  FOR c IN (SELECT table_name FROM user_tables)
  LOOP
    EXECUTE IMMEDIATE ('truncate table ' || c.table_name);
  END LOOP;
  FOR c IN (SELECT table_name, constraint_name FROM user_constraints WHERE constraint_type = 'R')
  LOOP
    EXECUTE IMMEDIATE ('alter table ' || c.table_name || ' enable constraint ' || c.constraint_name);
  END LOOP;
END SPCLEARSCHEMA;
/
--------------------------------------------------------
--  DDL for Table COMMAND
--------------------------------------------------------
CREATE TABLE TEST.COMMAND
	(COMMANDID RAW(16) NOT NULL,
	COMMANDTYPE VARCHAR2(250 CHAR) NOT NULL,
	BODY VARCHAR2(4000 CHAR) NOT NULL,
	BODYFORMAT VARCHAR2(20 CHAR) NOT NULL,
	RECURRINGEXPRESSION VARCHAR2(150 CHAR) NOT NULL,
	LASTEXECUTIONTIME TIMESTAMP(3) NULL,
	LASTEXECUTIONSTATUS CHAR(1 CHAR) NULL,
	LASTEXCEPTIONINFO VARCHAR2(4000 CHAR) NULL,
    CONSTRAINT PK_COMMAND PRIMARY KEY(COMMANDID))
/
--------------------------------------------------------
--  DDL for Index IX_COMMAND
--------------------------------------------------------
CREATE UNIQUE INDEX TEST.IX_COMMAND ON TEST.COMMAND (COMMANDTYPE)
/
--------------------------------------------------------
--  DDL for Table EVENT
--------------------------------------------------------

  CREATE TABLE TEST.EVENT 
   (EVENTID RAW(16) NOT NULL,
	EVENTTYPE VARCHAR2(250 CHAR) NOT NULL,
	OCCURREDON TIMESTAMP(3) NOT NULL,
	BODY VARCHAR2(4000 CHAR) NOT NULL,
	BODYFORMAT VARCHAR2(20 CHAR) NOT NULL,
	STREAMID VARCHAR2(50 CHAR) NOT NULL,
	STREAMTYPE VARCHAR2(50 CHAR) NOT NULL,
	ISSUEDBY VARCHAR2(100 CHAR),
	CONSTRAINT PK_EVENT PRIMARY KEY(EVENTID))
/
--------------------------------------------------------
--  DDL for Table EVENTSTREAM
--------------------------------------------------------

  CREATE TABLE TEST.EVENTSTREAM 
   (TYPE VARCHAR2(50 CHAR) NOT NULL, 
	SOURCE VARCHAR2(5 CHAR) NOT NULL, 
	POSITION RAW(16) NOT NULL,
	RETRYMAX NUMBER(3,0) NOT NULL, 
	RETRYDELAYS VARCHAR2(50 CHAR) NOT NULL,
	BLOCKSIZE NUMBER(5,0) NOT NULL,
	CONSTRAINT PK_EVENTSTREAM PRIMARY KEY(TYPE, SOURCE))
/
--------------------------------------------------------
--  DDL for Table FAILEDEVENTSTREAM
--------------------------------------------------------

  CREATE TABLE TEST.FAILEDEVENTSTREAM 
   (STREAMID VARCHAR2(50 CHAR) NOT NULL, 
	STREAMTYPE VARCHAR2(50 CHAR) NOT NULL, 
	STREAMSOURCE VARCHAR2(5 CHAR) NOT NULL, 
	STREAMPOSITION RAW(16) NOT NULL, 
	EVENTID RAW(16) NOT NULL, 
	EVENTTYPE VARCHAR2(250 CHAR) NOT NULL, 
	EXCEPTIONTIME TIMESTAMP(3) NOT NULL, 
	EXCEPTIONTYPE VARCHAR2(250 CHAR) NOT NULL, 
	EXCEPTIONMESSAGE VARCHAR2(4000 CHAR) NOT NULL, 
	EXCEPTIONSOURCE VARCHAR2(250 CHAR) NOT NULL,
	EXCEPTIONINFO VARCHAR2(4000 CHAR) NOT NULL, 
	BASEEXCEPTIONTYPE VARCHAR2(250 CHAR) NOT NULL, 
	BASEEXCEPTIONMESSAGE VARCHAR2(4000 CHAR) NOT NULL, 
	RETRYCOUNT NUMBER(3,0) NOT NULL, 
	RETRYMAX NUMBER(3,0) NOT NULL, 
	RETRYDELAYS VARCHAR2(50 CHAR),
	BLOCKSIZE NUMBER(5,0) NOT NULL,
	CONSTRAINT PK_FAILEDEVENTSTREAM PRIMARY KEY(STREAMID, STREAMTYPE, STREAMSOURCE))
/
--------------------------------------------------------
--  DDL for Table TABLEWITHID
--------------------------------------------------------

  CREATE TABLE TEST.TABLEWITHID 
   (ID NUMBER(10,0) NOT NULL,
    CONSTRAINT PK_TABLEWITHID PRIMARY KEY(ID))
/