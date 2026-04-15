INSERT INTO "ApplicationStatuses" ("Name")
VALUES
  ('На рассмотрении'),
  ('Приглашение на собеседование'),
  ('Отклонено'),
  ('Получен оффер'),
  ('Принято'),
  ('Трудоустроен'),
  ('Отозвано')
ON CONFLICT ("Name") DO NOTHING;