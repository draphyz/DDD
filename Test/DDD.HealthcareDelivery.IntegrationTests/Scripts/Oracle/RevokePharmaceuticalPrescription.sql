BEGIN
  SPCLEARSCHEMA();
END;
/
INSERT INTO TEST.Prescription (PrescriptionId, PrescriptionType, Status, Language, CreatedOn, DeliverableAt, PrescriberId, PrescriberType, PrescriberLastName, PrescriberFirstName, PrescriberDisplayName, PrescriberLicenseNum, PrescriberSSN, PrescriberSpeciality, PrescriberPhone1, PrescriberPhone2, PrescriberEmail1, PrescriberEmail2, PrescriberWebSite, PrescriberStreet, PrescriberHouseNum, PrescriberBoxNum, PrescriberPostCode, PrescriberCity, PrescriberCountry, PatientId, PatientFirstName, PatientLastName, PatientSex, PatientSSN, PatientBirthdate, EncounterId) VALUES (1, N'PHARM', N'CRT', N'FR', TO_DATE(N'2016-12-18','YYYY-MM-DD'), NULL, 1, N'Physician', N'Duck', N'Donald', N'Dr. Duck Donald', N'16480793370', NULL, N'Ophtalmologie', N'02/221.21.21', NULL, N'donald.duck@gmail.com', NULL, NULL, N'Grote Markt 7', NULL, NULL, N'1000', N'Brussel', NULL, 12601, N'Archibald', N'Haddock', N'M', NULL, TO_DATE(N'1940-12-12','YYYY-MM-DD'), 1)
/
INSERT INTO TEST.PrescMedication (PrescMedicationId, PrescriptionId, MedicationType, NameOrDesc, Posology, Quantity, Code) VALUES (1, 1, N'Product', N'Latansoc Mylan Coll. 2,5 ml X 3', N'1 goutte le soir', 1, NULL)
/
INSERT INTO TEST.PrescMedication (PrescMedicationId, PrescriptionId, MedicationType, NameOrDesc, Posology, Quantity, Code) VALUES (2, 1, N'Product', N'Dualkopt Coll. 10 ml', N'1 goutte 2 x/jour', 1, N'3260072')
/