CREATE EXTENSION pg_trgm;

create table "TransactionTypes"
(
    "Id" varchar(7) not null primary key
);

insert into "TransactionTypes"
values ('Income'),
       ('Expense');

create table "Currencies"
(
    "Id"   varchar(16) not null
        constraint "PK_Currencies"
            primary key,
    "Name" varchar(64) not null
);

create table "Users"
(
    "Id"                 uuid         not null
        constraint "PK_Users"
            primary key,
    "Name"               varchar(128) not null,
    "Email"              varchar(256) not null,
    "NormalizedEmail"    varchar(256) not null unique,
    "EmailConfirmed"     boolean      not null,
    "PasswordHash"       text,
    "SecurityStamp"      text,
    "ConcurrencyStamp"   text,
    "TwoFactorEnabled"   boolean      not null,
    "LockoutEnd"         timestamp with time zone,
    "LockoutEnabled"     boolean      not null,
    "AccessFailedCount"  integer      not null,
    "NormalizedUserName" varchar(256) unique,
    "UserName"           varchar(256)
);

create unique index "IX_Users_NormalizedUserName"
    on "Users" ("NormalizedUserName");

create unique index "IX_Users_NormalizedEmail"
    on "Users" ("NormalizedEmail");

create table "Budgets"
(
    "Id"         uuid        not null
        constraint "PK_Budgets"
            primary key,
    "Name"       varchar(64) not null,
    "CurrencyId" varchar(16) not null
        constraint "FK_Budgets_Currencies_CurrencyId"
            references "Currencies"
            on delete cascade,
    "Amount"     numeric     not null,
    "UserId"     uuid        not null
        constraint "FK_Budgets_Users_UserId"
            references "Users"
            on delete cascade,
    constraint "AK_Budgets_UserId_Name"
        unique ("UserId", "Name")
);

create index "IX_Budgets_CurrencyId"
    on "Budgets" ("CurrencyId");

create unique index "IX_Budgets_UserId_Name"
    on "Budgets" ("UserId", "Name");

create table "Wallets"
(
    "Id"             uuid                not null
        constraint "PK_Wallets"
            primary key,
    "Name"           varchar(64)         not null,
    "OwnerId"        uuid                not null
        constraint "FK_Wallets_Users_OwnerId"
            references "Users"
            on delete cascade,
    "CurrencyId"     varchar(16)         not null
        constraint "FK_Wallets_Currencies_CurrencyId"
            references "Currencies"
            on delete cascade,
    "StartingAmount" numeric default 0.0 not null,
    constraint "AK_Wallets_OwnerId_Name"
        unique ("OwnerId", "Name"),
    constraint "AK_Wallets_Id_OwnerId"
        unique ("Id", "OwnerId")
);

create index "IX_Wallets_CurrencyId"
    on "Wallets" ("CurrencyId");

create unique index "IX_Wallets_OwnerId_Name"
    on "Wallets" ("OwnerId", "Name");

create table "Categories"
(
    "Id"                  uuid        not null
        constraint "PK_Categories"
            primary key,
    "Name"                varchar(64) not null,
    "Appearance_Color"    varchar(64),
    "Appearance_IconName" varchar(64),
    "TransactionTypeId"   varchar(7)  not null
        constraint "FK_Categories_TransactionTypeId"
            references "TransactionTypes" on delete cascade,
    "WalletId"            uuid        not null
        constraint "FK_Categories_Wallets_WalletId"
            references "Wallets"
            on delete cascade,
    constraint "AK_Categories_Id_WalletId"
        unique ("Id", "WalletId", "TransactionTypeId"),
    constraint "AK_Categories_WalletId_Name"
        unique ("WalletId", "Name")
);

create unique index "IX_Categories_WalletId_Name"
    on "Categories" ("WalletId", "Name");

create index "IX_Categories_TransactionTypeId"
    on "Categories" ("TransactionTypeId");

create table "Labels"
(
    "Id"                  uuid        not null
        constraint "PK_Labels"
            primary key,
    "Name"                varchar(64) not null,
    "Appearance_Color"    varchar(64),
    "Appearance_IconName" varchar(64),
    "WalletId"            uuid        not null
        constraint "FK_Labels_Wallets_WalletId"
            references "Wallets"
            on delete cascade,
    constraint "AK_Labels_Id_WalletId"
        unique ("Id", "WalletId"),
    constraint "AK_Labels_WalletId_Name"
        unique ("WalletId", "Name")
);

create unique index "IX_Labels_WalletId_Name"
    on "Labels" ("WalletId", "Name");

create table "Recipients"
(
    "Id"       uuid        not null
        constraint "PK_Recipients"
            primary key,
    "Name"     varchar(64) not null,
    "WalletId" uuid        not null
        constraint "FK_Recipients_Wallets_WalletId"
            references "Wallets"
            on delete cascade,
    constraint "AK_Recipients_Id_WalletId"
        unique ("Id", "WalletId"),
    constraint "AK_Recipients_WalletId_Name"
        unique ("WalletId", "Name")
);

create unique index "IX_Recipients_WalletId_Name"
    on "Recipients" ("WalletId", "Name");

create table "UserWallet"
(
    "UserId"        uuid not null
        constraint "FK_UserWallet_Users_UserId"
            references "Users"
            on delete cascade,
    "WalletOwnerId" uuid not null,
    "WalletId"      uuid not null,
    constraint "FK_UserWallet_Wallets_WalletId_OwnerId"
        foreign key ("UserId", "WalletId") references "Wallets" ("Id", "OwnerId")
            on delete cascade,
    constraint "PK_UserWallet"
        primary key ("UserId", "WalletId"),
    constraint "CH_OwnerId_UserId"
        check ("UserId" != "WalletOwnerId")
);

create table "BudgetCategory"
(
    "BudgetId"   uuid not null
        constraint "FK_BudgetCategory_Budgets_BudgetId"
            references "Budgets"
            on delete cascade,
    "CategoryId" uuid not null
        constraint "FK_BudgetCategory_Categories_CategoryId"
            references "Categories"
            on delete cascade,
    constraint "PK_BudgetCategory"
        primary key ("BudgetId", "CategoryId")
);

create index "IX_BudgetCategory_CategoryId"
    on "BudgetCategory" ("CategoryId");

create table "Transactions"
(
    "Id"                        uuid         not null
        constraint "PK_Transactions"
            primary key,
    "Amount"                    numeric      not null,
    "Note"                      varchar(512) not null,
    "TransactionDate"           date         not null,
    "ExchangeRate"              numeric      not null,
    "RecipientWalletId"         uuid,
    "TransactionTypeId"         varchar(7)   not null
        constraint "FK_Transactions_TransactionTypeId"
            references "TransactionTypes" on delete cascade,
    "CategoryTransactionTypeId" varchar(7)   not null,
    "CategoryWalletId"          uuid,
    "CurrencyId"                varchar(16)  not null
        constraint "FK_Transactions_Currencies_CurrencyId"
            references "Currencies"
            on delete cascade,
    "WalletId"                  uuid         not null
        constraint "FK_Transactions_Wallets_WalletId"
            references "Wallets"
            on delete cascade,
    "CategoryId"                uuid,
    "RecipientId"               uuid,
    constraint "AK_Transactions_Id_WalletId"
        unique ("Id", "WalletId"),
    constraint "FK_Transactions_Categories_CategoryId_CategoryWalletId"
        foreign key ("CategoryId", "CategoryWalletId", "CategoryTransactionTypeId") references "Categories" ("Id", "WalletId", "TransactionTypeId")
            on delete set null,
    constraint "FK_Transactions_Recipients_RecipientId_RecipientWalletId"
        foreign key ("RecipientId", "RecipientWalletId") references "Recipients" ("Id", "WalletId")
            on delete set null,
    constraint "CH_WalletId_CategoryWalletId"
        check (("CategoryWalletId" IS NULL) OR ("CategoryWalletId" = "WalletId")),
    constraint "CH_WalletId_RecipientWalletId"
        check (("RecipientWalletId" IS NULL) OR ("RecipientWalletId" = "WalletId")),
    constraint "CH_TransactionTypeId_CategoryTransactionTypeId"
        check (("CategoryTransactionTypeId" IS NULL) OR ("TransactionTypeId" = "CategoryTransactionTypeId"))
);

create index "IX_Transactions_WalletId"
    on "Transactions" ("WalletId");

create index "IX_Transactions_CurrencyId"
    on "Transactions" ("CurrencyId");

create index "IX_Transactions_TransactionTypeId"
    on "Transactions" ("TransactionTypeId");

create table "TransactionLabel"
(
    "LabelId"             uuid not null,
    "LabelWalletId"       uuid not null,
    "TransactionId"       uuid not null,
    "TransactionWalletId" uuid not null,
    constraint "PK_TransactionLabel"
        primary key ("LabelId", "TransactionId"),
    constraint "FK_TransactionLabel_Labels_LabelId_LabelWalletId"
        foreign key ("LabelId", "LabelWalletId") references "Labels" ("Id", "WalletId")
            on delete cascade,
    constraint "FK_TransactionLabel_Transactions_TransactionId_TransactionWall~"
        foreign key ("TransactionId", "TransactionWalletId") references "Transactions" ("Id", "WalletId")
            on delete cascade,
    constraint "CH_TransactionLabel_TransactionWalletId_LabelWalletId"
        check ("TransactionWalletId" = "LabelWalletId")
);

create view "FullSearchTransactions"
            ("Id", "Note", "Amount", "CategoryName", "RecipientName", "CurrencyId", "CurrencyName", "WalletId",
             "TransactionDate", "Labels")
as
SELECT t."Id",
       t."Note",
       t."Amount",
       c."Name"                                                                   AS "CategoryName",
       r."Name"                                                                   AS "RecipientName",
       cr."Id"                                                                    AS "CurrencyId",
       cr."Name"                                                                  AS "CurrencyName",
       t."WalletId",
       to_char(t."TransactionDate"::timestamp with time zone, 'DD.MM.YYYY'::text) AS "TransactionDate",
       l."Labels"
FROM "Transactions" t
         LEFT JOIN "Categories" c ON t."CategoryId" = c."Id"
         LEFT JOIN "Recipients" r ON t."RecipientId" = r."Id"
         LEFT JOIN "Currencies" cr ON t."CurrencyId"::text = cr."Id"::text
         LEFT JOIN (SELECT xlt."TransactionId",
                           string_agg(xl."Name"::text, ', '::text) AS "Labels"
                    FROM "Labels" xl
                             LEFT JOIN "TransactionLabel" xlt ON xl."Id" = xlt."LabelId"
                    GROUP BY xlt."TransactionId") l ON l."TransactionId" = t."Id";

create function "SearchUserTransactions"(userid uuid, searchterm character varying,
                                         walletid uuid DEFAULT NULL::uuid) returns SETOF "Transactions"
    language plpgsql
as
$$
BEGIN
    RETURN QUERY SELECT *
                 FROM "Transactions"
                 WHERE "Transactions"."Id" IN (SELECT FST."Id"
                                               FROM (SELECT *
                                                     FROM "FullSearchTransactions"
                                                     WHERE (walletId is null or "FullSearchTransactions"."WalletId" = walletId)
                                                       and "FullSearchTransactions"."WalletId" IN
                                                           (SELECT "UserWallet"."WalletId"
                                                            FROM "UserWallet"
                                                            WHERE "UserWallet"."UserId" = userId
                                                            UNION
                                                            SELECT "Wallets"."Id"
                                                            FROM "Wallets"
                                                            WHERE "Wallets"."OwnerId" = userId)) FST,
                                                    SIMILARITY(searchTerm,
                                                               coalesce(FST."Note", '') ||
                                                               coalesce(cast(FST."Amount" as varchar), '') ||
                                                               coalesce(FST."TransactionDate", '') ||
                                                               coalesce(FST."RecipientName", '') ||
                                                               coalesce(FST."CategoryName", '') ||
                                                               coalesce(FST."CurrencyName", '') ||
                                                               coalesce(FST."CurrencyId", '') ||
                                                               coalesce(FST."Labels", '')) similarity
                                               WHERE similarity > 0
                                               ORDER BY similarity DESC NULLS LAST);
END
$$;

create procedure "GenerateDefaultCategories"(IN walletid uuid)
    language plpgsql
as
$$
BEGIN
    INSERT INTO "Categories" ("Name", "Appearance_Color", "Appearance_IconName", "TransactionTypeId", "WalletId")
    VALUES ('Uncategorized income', 'icon-support-01', 'fa::wallet', 'Income', walletId),
           ('Salary', 'icon-support-02', 'fa::money-bill', 'Income', walletId),
           ('Uncategorized expense', 'icon-support-01', 'fa::sack', 'Expense', walletId),
           ('Food', 'icon-support-03', 'fa::utensils', 'Expense', walletId),
           ('Commute', 'icon-support-04', 'fa:train', 'Expense', walletId),
           ('Household chemistry', 'icon-support-05', 'fa::soap', 'Expense', walletId),
           ('Dining out', 'icon-support-06', 'fa::pizza-slice', 'Expense', walletId),
           ('Rent', 'icon-support-07', 'fa::building', 'Expense', walletId);
END
$$;

-- CREATE OR REPLACE FUNCTION "SearchUserTransactions"(userId uuid, searchTerm VARCHAR, walletId uuid DEFAULT null)
--     RETURNS SETOF "Transactions"
--     LANGUAGE plpgsql AS
-- $$
-- BEGIN
--     RETURN QUERY SELECT *
--                  FROM "Transactions"
--                  WHERE "Transactions"."Id" IN (SELECT FST."Id"
--                                                FROM (SELECT *
--                                                      FROM "FullSearchTransactions"
--                                                      WHERE "FullSearchTransactions"."WalletId" IN
--                                                            (SELECT "WalletAccesses"."WalletId"
--                                                             FROM "WalletAccesses"
--                                                             WHERE "WalletAccesses"."UserId" = userId
--                                                               and (walletId is null or "WalletAccesses"."WalletId" = walletId))) FST,
-- --                                                     to_tsvector(coalesce(FST."Note", '') ||
-- --                                                                 coalesce(cast(FST."Amount" as varchar), '') ||
-- --                                                                 coalesce(FST."RecipientName", '') ||
-- --                                                                 coalesce(FST."CategoryName", '') ||
-- --                                                                 coalesce(FST."CurrencyName", '') ||
-- --                                                                 coalesce(FST."CurrencyCode", '') ||
-- --                                                                 coalesce(FST."Labels", '')) tsv,
-- --                                                     to_tsquery(searchTerm) tsq,
-- --                                                     NULLIF(ts_rank(to_tsvector(coalesce(FST."Note", '')), tsq), 0) rank_note,
-- --                                                     NULLIF(ts_rank(to_tsvector(coalesce(FST."TransactionDate", '')), tsq), 0) rank_date,
-- --                                                     NULLIF(ts_rank(to_tsvector(coalesce(FST."Labels", '')), tsq), 0) rank_labels,
-- --                                                     NULLIF(ts_rank(to_tsvector(coalesce(FST."RecipientName", '')), tsq), 0) rank_recipient,
-- --                                                     NULLIF(ts_rank(to_tsvector(coalesce(FST."CategoryName", '')), tsq), 0) rank_category,
--                                                     SIMILARITY(searchTerm,
--                                                                coalesce(FST."Note", '') ||
--                                                                coalesce(cast(FST."Amount" as varchar), '') ||
--                                                                coalesce(FST."TransactionDate", '') ||
--                                                                coalesce(FST."RecipientName", '') ||
--                                                                coalesce(FST."CategoryName", '') ||
--                                                                coalesce(FST."CurrencyName", '') ||
--                                                                coalesce(FST."CurrencyCode", '') ||
--                                                                coalesce(FST."Labels", '')) similarity
--                                                WHERE
-- --                                                    tsq @@ tsv or
--                                                    similarity > 0
--                                                ORDER BY
-- --                                                    rank_note, rank_date, rank_labels, rank_recipient, rank_category,
--                                                    similarity DESC NULLS LAST);
-- END
-- $$;
