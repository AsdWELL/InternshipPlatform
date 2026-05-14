INSERT INTO "Roles" ("Name")
VALUES 
    ('Student'),
    ('Employer'),
    ('Teacher')
ON CONFLICT ("Name") DO NOTHING;