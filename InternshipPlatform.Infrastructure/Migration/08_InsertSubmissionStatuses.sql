INSERT INTO "PracticeSubmissionStatuses" ("Name")
VALUES
    ('Отправлено'),
    ('Требуются исправления')
    ('Решение принято'),
    ('Оценено')
ON CONFLICT ("Name") DO NOTHING;