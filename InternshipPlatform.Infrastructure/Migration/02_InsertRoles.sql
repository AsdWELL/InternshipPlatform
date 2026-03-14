INSERT INTO "Roles" ("Name")
VALUES 
    ('Student'),
    ('Employer')
ON CONFLICT ("Name") DO NOTHING;