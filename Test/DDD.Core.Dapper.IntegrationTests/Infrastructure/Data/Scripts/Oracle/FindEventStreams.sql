BEGIN
  SPCLEARSCHEMA();
END;
/
INSERT INTO TEST.EventStream(Type, Source, Position, RetryMax, RetryDelays, BlockSize) VALUES ('Person', 'ID', 'D05BDFF763877E677E6B3A0044746810', 5, '10,60,120/80', 50)
/																   																   
INSERT INTO TEST.EventStream(Type, Source, Position, RetryMax, RetryDelays, BlockSize) VALUES ('MedicalProduct', 'OFR', '00000000000000000000000000000000', 3, '60', 100)
/																			   