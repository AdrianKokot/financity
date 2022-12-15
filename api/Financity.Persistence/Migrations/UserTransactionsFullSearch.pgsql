CREATE EXTENSION pg_trgm;

DROP VIEW "FullSearchTransactions";
CREATE VIEW "FullSearchTransactions" AS
SELECT T."Id",
       T."Note",
       T."Amount",
       C."Name"                                   "CategoryName",
       R."Name"                                   "RecipientName",
       CR."Id"                                    "CurrencyId",
       CR."Name"                                  "CurrencyName",
       T."WalletId"                               "WalletId",
       to_char(T."TransactionDate", 'DD.MM.YYYY') "TransactionDate",
       L."Labels"
FROM "Transactions" T
         LEFT JOIN "Categories" C on T."CategoryId" = C."Id"
         LEFT JOIN "Recipients" R on T."RecipientId" = R."Id"
         LEFT JOIN "Currencies" CR on T."CurrencyId" = CR."Id"
         LEFT JOIN (SELECT XLT."TransactionId" "TransactionId", string_agg(XL."Name", ', ') "Labels"
                    FROM "Labels" XL
                             LEFT JOIN "TransactionLabel" XLT on XL."Id" = XLT."LabelId"
                    GROUP BY XLT."TransactionId") L on L."TransactionId" = T."Id";

CREATE OR REPLACE FUNCTION "SearchUserTransactions"(userId uuid, searchTerm VARCHAR, walletId uuid DEFAULT null)
    RETURNS SETOF "Transactions"
    LANGUAGE plpgsql AS
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


CREATE OR REPLACE PROCEDURE "GenerateDefaultCategories"(walletId uuid)
LANGUAGE PLPGSQL
AS $$
BEGIN
    INSERT INTO "Categories" ("Name", "Appearance_Color", "Appearance_IconName", "TransactionType", "WalletId")
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
