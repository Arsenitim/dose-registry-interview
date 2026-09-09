-- Dose Registry schema + seed data.
--
-- This runs once, automatically, the first time the `db` container starts
-- with an empty data volume (Postgres's docker-entrypoint-initdb.d
-- convention). If you change this file after the volume already exists,
-- run `docker compose down -v` first so it re-initializes.
--
-- There is deliberately no ORM migration tooling here: schema is owned by
-- this SQL script, and the application's DbContext maps onto these
-- pre-existing tables via Fluent API.

CREATE TABLE workers (
    id              SERIAL PRIMARY KEY,
    full_name       TEXT NOT NULL,
    personal_number TEXT NOT NULL UNIQUE
);

CREATE TABLE dose_records (
    id              SERIAL PRIMARY KEY,
    worker_id       INTEGER NOT NULL REFERENCES workers(id),
    period_start    DATE NOT NULL,
    period_end      DATE NOT NULL,
    dose_value_msv  NUMERIC(10,4) NOT NULL,
    CONSTRAINT dose_records_period_valid CHECK (period_end >= period_start)
);

CREATE INDEX idx_dose_records_worker_id ON dose_records(worker_id);
CREATE INDEX idx_dose_records_period ON dose_records(period_start, period_end);

-- Sample data: 5 workers, 8 dose records.

INSERT INTO workers (id, full_name, personal_number) VALUES
  (1, 'Anna Novak',      'W-1001'),
  (2, 'Marek Dvorak',    'W-1002'),
  (3, 'Julia Kowalski',  'W-1003'),
  (4, 'Tomas Novotny',   'W-1004'),
  (5, 'Elena Rossi',     'W-1005');

SELECT setval('workers_id_seq', 5);

INSERT INTO dose_records (worker_id, period_start, period_end, dose_value_msv) VALUES
  (1, '2026-02-01', '2026-02-28', 4.0),
  (1, '2026-08-01', '2026-08-31', 3.0),
  (2, '2025-12-20', '2026-01-15', 3.5),
  (3, '2026-12-18', '2027-01-10', 2.2),
  (4, '2026-01-10', '2026-03-10', 8.0),
  (4, '2026-04-01', '2026-06-30', 9.0),
  (4, '2026-07-01', '2026-09-30', 6.0),
  (5, '2026-03-01', '2026-04-15', 20.0);
