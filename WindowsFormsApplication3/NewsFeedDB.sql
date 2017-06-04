BEGIN TRANSACTION;
CREATE TABLE "video_table" (
	`video_id`	INTEGER,
	`video_name`	TEXT,
	`video_path`	TEXT,
	`notes`	TEXT,
	PRIMARY KEY(video_id)
);
INSERT INTO `video_table` VALUES ('1','Siemens News','C:/Users/News',NULL);
INSERT INTO `video_table` VALUES ('2','Nokia News','C:/Users/News',NULL);
CREATE TABLE "source_table" (
	`source_id`	INTEGER,
	`source_name`	TEXT,
	`is_reliable`	TEXT,
	`video_id`	INTEGER,
	PRIMARY KEY(source_id,source_name,video_id)
);
INSERT INTO `source_table` VALUES ('1','Youtube
','false','123');
INSERT INTO `source_table` VALUES ('1','Youtube
','false','234');
INSERT INTO `source_table` VALUES ('2','Daily Motion','false','567');
INSERT INTO `source_table` VALUES ('3','Putlocker','true','678');
INSERT INTO `source_table` VALUES ('3','Putlocker','true','0');
INSERT INTO `source_table` VALUES ('4','SolarMovies','false','123');
CREATE TABLE "snippet_table" (
	`snippet_id`	INTEGER,
	`video_id`	INTEGER,
	`snippet_path`	TEXT,
	`order`	INTEGER,
	`to_timestamp`	TEXT,
	`from_timestamp`	TEXT,
	PRIMARY KEY(snippet_id,video_id)
);
CREATE TABLE `newsfeed_table` (
	`newsfeed_id`	INTEGER,
	`snipped_id`	INTEGER,
	`newsfeed_path`	TEXT,
	`is_translated`	INTEGER,
	PRIMARY KEY(newsfeed_id,snipped_id)
);
;
;
;
COMMIT;
