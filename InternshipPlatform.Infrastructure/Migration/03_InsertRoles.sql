INSERT INTO "Roles" ("Name")
VALUES 
    ('Student'),
    ('Employer'),
    ('Curator')
ON CONFLICT ("Name") DO NOTHING;