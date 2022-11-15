CREATE EXTENSION pg_trgm;

DROP VIEW "FullSearchTransactions";
CREATE VIEW "FullSearchTransactions" AS
SELECT T."Id",
       T."Note",
       T."Amount",
       C."Name"  "CategoryName",
       R."Name"  "RecipientName",
       CR."Code" "CurrencyCode",
       CR."Name" "CurrencyName",
       T."WalletId" "WalletId",
       L."Labels"
FROM "Transactions" T
         LEFT JOIN "Categories" C on T."CategoryId" = C."Id"
         LEFT JOIN "Recipients" R on T."RecipientId" = R."Id"
         LEFT JOIN "Currencies" CR on T."CurrencyId" = CR."Id"
         LEFT JOIN (SELECT XLT."TransactionsId" "TransactionId", string_agg(XL."Name", ' ') "Labels"
                    FROM "Labels" XL
                             LEFT JOIN "LabelTransaction" XLT on XL."Id" = XLT."LabelsId"
                    GROUP BY XLT."TransactionsId") L on L."TransactionId" = T."Id";

CREATE OR REPLACE FUNCTION "UserTransactionsFullSearch"(userId uuid, searchTerm VARCHAR)
    RETURNS TABLE
            (
                "Id"              uuid,
                "Amount"          numeric,
                "Note"            text,
                "RecipientId"     uuid,
                "WalletId"        uuid,
                "TransactionType" integer,
                "CategoryId"      uuid,
                "CurrencyId"      uuid,
                "CreatedAt"       timestamp with time zone,
                "CreatedBy"       uuid,
                "UpdatedAt"       timestamp with time zone,
                "UpdateBy"        uuid
            )
AS
$$
BEGIN
    RETURN QUERY SELECT *
                 FROM "Transactions"
                 WHERE "Transactions"."Id" IN (SELECT FST."Id"
                                FROM (SELECT *
                                      FROM "FullSearchTransactions"
                                      WHERE "FullSearchTransactions"."WalletId" IN (SELECT "WalletAccesses"."WalletId"
                                                                                    FROM "WalletAccesses"
                                                                                    WHERE "WalletAccesses"."UserId" = userId)) FST,
                                     to_tsvector(FST."Note" || FST."Amount" || FST."RecipientName" ||
                                                 FST."CategoryName" || FST."CurrencyName" || FST."CurrencyCode" ||
                                                 FST."Labels") tsv,
                                     to_tsquery(searchTerm) tsq,
                                     SIMILARITY(searchTerm, FST."Note" || FST."Amount" || FST."RecipientName" ||
                                                 FST."CategoryName" || FST."CurrencyName" || FST."CurrencyCode" ||
                                                 FST."Labels") similarity
                                WHERE tsq @@ tsv or similarity > 0);
END;
$$
    LANGUAGE 'plpgsql';

SELECT *
FROM "UserTransactionsFullSearch"((SELECT "Id" FROM "Users"), 'testlabel');