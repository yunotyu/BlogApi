/*
 Navicat Premium Data Transfer

 Source Server         : mysql
 Source Server Type    : MySQL
 Source Server Version : 80022
 Source Host           : localhost:3306
 Source Schema         : blogsql

 Target Server Type    : MySQL
 Target Server Version : 80022
 File Encoding         : 65001

 Date: 23/04/2023 17:57:09
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for advertisement
-- ----------------------------
DROP TABLE IF EXISTS `advertisement`;
CREATE TABLE `advertisement`  (
  `id` bigint(0) NOT NULL AUTO_INCREMENT,
  `title` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '广告标题',
  `imgUrl` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '广告图片',
  `url` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '广告链接',
  `remark` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `createTime` datetime(0) NULL DEFAULT NULL,
  `isDel` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for article
-- ----------------------------
DROP TABLE IF EXISTS `article`;
CREATE TABLE `article`  (
  `id` bigint(0) NOT NULL AUTO_INCREMENT,
  `createUsername` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '发布文章的用户名',
  `articleTitle` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '文章标题',
  `articleKind` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '文章分类，以--分隔',
  `articleContent` varchar(20000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '文章内容，注意长度限制，6000字以下',
  `createTime` datetime(0) NULL DEFAULT NULL COMMENT '文章发布时间',
  `lookCount` int(0) NULL DEFAULT NULL COMMENT '文章访问次数',
  `likeCount` int(0) NULL DEFAULT NULL COMMENT '点赞数',
  `commentCount` int(0) NULL DEFAULT NULL COMMENT '评论数',
  `isDel` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of article
-- ----------------------------
INSERT INTO `article` VALUES (1, '但是福建省', '发奖金阿迪斯', '怪卡的', '加速度', '2023-04-04 16:34:36', NULL, NULL, NULL, 0);

-- ----------------------------
-- Table structure for collection
-- ----------------------------
DROP TABLE IF EXISTS `collection`;
CREATE TABLE `collection`  (
  `id` bigint(0) NOT NULL AUTO_INCREMENT,
  `articleId` bigint(0) NULL DEFAULT NULL COMMENT '文章id',
  `createUsername` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `createTime` datetime(0) NULL DEFAULT NULL,
  `isDel` tinyint(1) NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for comments
-- ----------------------------
DROP TABLE IF EXISTS `comments`;
CREATE TABLE `comments`  (
  `id` bigint(0) NOT NULL AUTO_INCREMENT,
  `articleId` bigint(0) NOT NULL COMMENT '外键，关联对应的文章',
  `comment` varchar(1200) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '评论，500字以内',
  `createTime` datetime(0) NULL DEFAULT NULL,
  `createName` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT '游客' COMMENT '可以为空，默认为游客访问',
  `isDel` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of comments
-- ----------------------------
INSERT INTO `comments` VALUES (1, 2, '十点多撒', '2023-04-04 16:34:55', '阿萨德', 0);

-- ----------------------------
-- Table structure for exceptionlog
-- ----------------------------
DROP TABLE IF EXISTS `exceptionlog`;
CREATE TABLE `exceptionlog`  (
  `id` bigint(0) NOT NULL AUTO_INCREMENT,
  `time` datetime(0) NULL DEFAULT NULL,
  `threadId` int(0) NOT NULL COMMENT '异常线程id',
  `level` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '异常等级',
  `message` varchar(2000) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL COMMENT '异常信息',
  `exceptionType` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL COMMENT '异常类型',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for menus
-- ----------------------------
DROP TABLE IF EXISTS `menus`;
CREATE TABLE `menus`  (
  `id` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `menuNames` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `parentId` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT 'parentId 为0说明是根节点',
  `depth` int(0) NULL DEFAULT NULL COMMENT '根节点下面的最大子节点的长度',
  `url` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL COMMENT '对应的路由',
  `isDel` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 11 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of menus
-- ----------------------------
INSERT INTO `menus` VALUES ('9309e615-22cc-4de5-809e-abfc89b963dd', '用户管理', NULL, NULL, '/home', 0);
INSERT INTO `menus` VALUES ('a98e83d7-e855-4b7a-8949-f2e6d828eb92', '用户管理2', '9309e615-22cc-4de5-809e-abfc89b963dd', NULL, '/home2', 0);
INSERT INTO `menus` VALUES ('d745c055-be08-4b02-8627-411f86d3fb47', '用户管理22', 'a98e83d7-e855-4b7a-8949-f2e6d828eb92', NULL, '/home22', 0);
INSERT INTO `menus` VALUES ('f604c1eb-1b84-4f9a-98e7-43efc23f837d', '用户管理3', '9309e615-22cc-4de5-809e-abfc89b963dd', NULL, '/home3', 0);

-- ----------------------------
-- Table structure for operatelog
-- ----------------------------
DROP TABLE IF EXISTS `operatelog`;
CREATE TABLE `operatelog`  (
  `id` bigint(0) NOT NULL AUTO_INCREMENT,
  `area` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '访问的区域',
  `controllerName` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '访问的控制器名',
  `actionName` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '访问的action方法',
  `ip` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '来访的ip',
  `logTime` datetime(0) NULL DEFAULT NULL COMMENT '访问的时间',
  `username` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '可以为空，如果是没登录的用户',
  `isDel` tinyint(1) NOT NULL DEFAULT 0 COMMENT '0:未删除，1:已删除',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for permission
-- ----------------------------
DROP TABLE IF EXISTS `permission`;
CREATE TABLE `permission`  (
  `id` bigint(0) NOT NULL AUTO_INCREMENT,
  `icon` varchar(1000) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '菜单图标',
  `isBtn` tinyint(1) NULL DEFAULT 0 COMMENT '是否显示菜单为按钮，0代表不显示，1显示',
  `isShow` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否显示该菜单项，0代表不显示，1代表显示',
  `description` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `createName` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `createTime` datetime(0) NULL DEFAULT NULL,
  `modifyName` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '修改人的用户名',
  `modifyTime` datetime(0) NULL DEFAULT NULL COMMENT '修改时间',
  `menuId` bigint(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 13 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of permission
-- ----------------------------
INSERT INTO `permission` VALUES (2, 'xxxx', 1, 0, 'string', '', '2023-04-21 14:24:52', '', '2023-04-21 14:24:52', 1);
INSERT INTO `permission` VALUES (3, 'ggg', 1, 0, 'string', '', '2023-04-21 14:24:52', '', '2023-04-21 14:24:52', 2);
INSERT INTO `permission` VALUES (4, NULL, 0, 0, NULL, 'admin', '2023-04-04 17:32:16', NULL, NULL, 3);
INSERT INTO `permission` VALUES (5, NULL, 0, 0, NULL, 'admin', '2023-04-13 13:44:52', NULL, NULL, 4);
INSERT INTO `permission` VALUES (9, '/home.png', 0, 1, 'home', '', '2023-04-21 14:59:41', '', '2023-04-21 14:59:41', 2);
INSERT INTO `permission` VALUES (10, '/home2.png', 0, 1, 'home', '', '2023-04-21 15:00:56', '', '2023-04-21 15:00:56', 2);
INSERT INTO `permission` VALUES (11, '/menu.png', 0, 1, 'menu', '', '2023-04-21 15:00:56', '', '2023-04-21 15:00:56', 3);

-- ----------------------------
-- Table structure for permissonmenu
-- ----------------------------
DROP TABLE IF EXISTS `permissonmenu`;
CREATE TABLE `permissonmenu`  (
  `id` bigint(0) NOT NULL AUTO_INCREMENT,
  `permissonId` bigint(0) NULL DEFAULT NULL,
  `menuId` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `modifyName` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `modifyTime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of permissonmenu
-- ----------------------------
INSERT INTO `permissonmenu` VALUES (1, 2, '9309e615-22cc-4de5-809e-abfc89b963dd', NULL, NULL);
INSERT INTO `permissonmenu` VALUES (2, 3, 'a98e83d7-e855-4b7a-8949-f2e6d828eb92', NULL, NULL);
INSERT INTO `permissonmenu` VALUES (3, 4, 'd745c055-be08-4b02-8627-411f86d3fb47', NULL, NULL);

-- ----------------------------
-- Table structure for role
-- ----------------------------
DROP TABLE IF EXISTS `role`;
CREATE TABLE `role`  (
  `id` bigint(0) NOT NULL AUTO_INCREMENT,
  `roleName` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL COMMENT '角色名',
  `description` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '角色描述',
  `level` int(0) NULL DEFAULT NULL COMMENT '角色等级',
  `enable` tinyint(1) NOT NULL DEFAULT 1 COMMENT '是否启用，1代表启用，0代表不启用',
  `isDel` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否逻辑删除，0代表未删除，1代表删除',
  `createUsername` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL COMMENT '创建的用户名',
  `createTime` datetime(0) NOT NULL COMMENT '创建时间',
  `modifyTime` datetime(0) NULL DEFAULT NULL COMMENT '修改时间',
  `modifyUsername` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of role
-- ----------------------------
INSERT INTO `role` VALUES (1, 'admin', '管理员', NULL, 1, 0, '管理员', '2023-04-04 17:05:16', NULL, NULL);
INSERT INTO `role` VALUES (2, 'user', '普通用户', NULL, 1, 0, '管理员', '2023-04-12 15:43:33', NULL, NULL);
INSERT INTO `role` VALUES (3, 'gggg', 'vvvv', NULL, 1, 1, '', '2023-04-20 17:45:54', '2023-04-20 17:45:54', NULL);

-- ----------------------------
-- Table structure for rolepermission
-- ----------------------------
DROP TABLE IF EXISTS `rolepermission`;
CREATE TABLE `rolepermission`  (
  `id` bigint(0) NOT NULL AUTO_INCREMENT,
  `roleId` bigint(0) NOT NULL COMMENT '外键，角色表',
  `permissionId` bigint(0) NOT NULL COMMENT '外键，permission表',
  `isDel` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否逻辑删除，0代表未删除，1代表删除',
  `createUsername` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `createTime` datetime(0) NULL DEFAULT NULL,
  `modifyUsername` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `modifyTime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `roleForKey`(`roleId`) USING BTREE,
  INDEX `perForKey`(`permissionId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of rolepermission
-- ----------------------------
INSERT INTO `rolepermission` VALUES (1, 1, 2, 0, 'admin', '2023-04-04 17:34:01', NULL, NULL);
INSERT INTO `rolepermission` VALUES (2, 1, 3, 0, 'admin', '2023-04-04 17:34:19', NULL, NULL);
INSERT INTO `rolepermission` VALUES (3, 1, 4, 0, 'admin', '2023-04-04 17:34:39', NULL, NULL);
INSERT INTO `rolepermission` VALUES (4, 2, 5, 0, 'admin', '2023-04-13 13:45:14', NULL, NULL);

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user`  (
  `id` bigint(0) NOT NULL AUTO_INCREMENT,
  `username` varchar(25) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `pwd` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `createTime` datetime(0) NOT NULL COMMENT '创建时间',
  `updateTime` datetime(0) NULL DEFAULT NULL COMMENT '修改信息时间',
  `lastErrorTime` datetime(0) NULL DEFAULT NULL COMMENT '上次密码错误时间',
  `errorCount` int(0) NULL DEFAULT NULL COMMENT '密码错误次数',
  `remark` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '备注',
  `sex` int(0) NULL DEFAULT NULL COMMENT '性别 0:男  1女',
  `age` int(0) NULL DEFAULT NULL COMMENT '年龄',
  `birth` datetime(6) NULL DEFAULT NULL COMMENT '生日',
  `isDel` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否逻辑删除，0代表未删除，1代表删除',
  `isEnable` tinyint(1) NOT NULL DEFAULT 1 COMMENT '是否启用，1代表启用，0代表不启用',
  `modifyName` varchar(25) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '修改信息用户名',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 28 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of user
-- ----------------------------
INSERT INTO `user` VALUES (5, 'yu', '202CB962AC59075B964B07152D234B70', '2023-04-13 09:38:30', '2023-04-13 09:38:30', '2023-04-13 09:38:30', 0, 'ggg', 1, 20, '2023-04-17 15:25:38.298000', 0, 1, '');
INSERT INTO `user` VALUES (6, 'admin', '21232F297A57A5A743894A0E4A801FC3', '2023-04-13 10:57:35', NULL, NULL, NULL, 'xxxx', 0, 0, '2023-04-13 00:00:00.000000', 0, 1, NULL);
INSERT INTO `user` VALUES (9, 'y1', '202CB962AC59075B964B07152D234B70', '2023-04-17 11:28:16', NULL, NULL, NULL, NULL, 0, 0, '2023-04-17 11:28:06.818000', 0, 1, NULL);
INSERT INTO `user` VALUES (10, 'y2', '202CB962AC59075B964B07152D234B70', '2023-04-17 11:28:33', NULL, NULL, NULL, NULL, 0, 0, '2023-04-17 11:28:06.818000', 0, 1, NULL);
INSERT INTO `user` VALUES (11, 'y3', '202CB962AC59075B964B07152D234B70', '2023-04-17 11:28:51', NULL, NULL, NULL, NULL, 0, 0, '2023-04-17 11:28:06.818000', 0, 1, NULL);
INSERT INTO `user` VALUES (12, 'y4', '202CB962AC59075B964B07152D234B70', '2023-04-17 11:28:54', NULL, NULL, NULL, NULL, 0, 0, '2023-04-17 11:28:06.818000', 0, 1, NULL);
INSERT INTO `user` VALUES (13, 'y5', '202CB962AC59075B964B07152D234B70', '2023-04-17 11:28:58', NULL, NULL, NULL, NULL, 0, 0, '2023-04-17 11:28:06.818000', 0, 1, NULL);
INSERT INTO `user` VALUES (14, 'y6', '202CB962AC59075B964B07152D234B70', '2023-04-17 11:29:06', NULL, NULL, NULL, NULL, 0, 0, '2023-04-17 11:28:06.818000', 0, 1, NULL);
INSERT INTO `user` VALUES (15, 'y7', '202CB962AC59075B964B07152D234B70', '2023-04-17 11:29:11', NULL, NULL, NULL, NULL, 0, 0, '2023-04-17 11:28:06.818000', 0, 1, NULL);
INSERT INTO `user` VALUES (16, 'y8', '202CB962AC59075B964B07152D234B70', '2023-04-17 11:29:17', NULL, NULL, NULL, NULL, 0, 0, '2023-04-17 11:28:06.818000', 0, 1, NULL);
INSERT INTO `user` VALUES (17, 'y9', '202CB962AC59075B964B07152D234B70', '2023-04-17 11:29:22', NULL, NULL, NULL, NULL, 0, 0, '2023-04-17 11:28:06.818000', 0, 1, NULL);
INSERT INTO `user` VALUES (18, 'y11', '202CB962AC59075B964B07152D234B70', '2023-04-17 11:29:27', NULL, NULL, NULL, NULL, 0, 0, '2023-04-17 11:28:06.818000', 0, 1, NULL);
INSERT INTO `user` VALUES (23, 'admidn', '550A141F12DE6341FBA65B0AD0433500', '2023-04-19 17:31:57', NULL, NULL, NULL, NULL, 0, 0, '2023-04-19 17:25:39.675000', 0, 1, '');
INSERT INTO `user` VALUES (24, 'yue', '202CB962AC59075B964B07152D234B70', '2023-04-19 17:31:57', NULL, NULL, NULL, NULL, 0, 0, '2023-04-19 17:25:39.675000', 0, 0, '');
INSERT INTO `user` VALUES (25, 'yur', 'B45CFFE084DD3D20D928BEE85E7B0F21', '2023-04-19 17:31:57', NULL, NULL, NULL, NULL, 0, 0, '2023-04-19 17:25:39.675000', 0, 0, '');
INSERT INTO `user` VALUES (26, 'ff', 'F7177163C833DFF4B38FC8D2872F1EC6', '2023-04-20 10:05:44', NULL, NULL, NULL, NULL, 0, 0, '2023-04-20 10:05:22.941000', 0, 1, NULL);
INSERT INTO `user` VALUES (27, 'string', 'B45CFFE084DD3D20D928BEE85E7B0F21', '2023-04-20 15:49:49', NULL, NULL, NULL, NULL, 0, 0, '2023-04-20 15:49:45.482000', 0, 1, NULL);

-- ----------------------------
-- Table structure for userrole
-- ----------------------------
DROP TABLE IF EXISTS `userrole`;
CREATE TABLE `userrole`  (
  `id` bigint(0) NOT NULL AUTO_INCREMENT,
  `userId` bigint(0) NOT NULL COMMENT '外键，关联user表id',
  `roleId` bigint(0) NOT NULL COMMENT '外键，关联角色表id',
  `createUsername` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `createTime` datetime(0) NULL DEFAULT NULL,
  `modifyUsername` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `modifyTime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `useridForKey`(`userId`) USING BTREE,
  INDEX `roleidForKey`(`roleId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 27 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of userrole
-- ----------------------------
INSERT INTO `userrole` VALUES (5, 5, 1, NULL, '2023-04-13 10:19:26', NULL, NULL);
INSERT INTO `userrole` VALUES (7, 5, 2, NULL, '2023-04-13 11:09:18', NULL, NULL);
INSERT INTO `userrole` VALUES (8, 9, 2, NULL, '2023-04-17 11:28:21', NULL, NULL);
INSERT INTO `userrole` VALUES (9, 10, 2, NULL, '2023-04-17 11:28:33', NULL, NULL);
INSERT INTO `userrole` VALUES (10, 11, 2, NULL, '2023-04-17 11:28:51', NULL, NULL);
INSERT INTO `userrole` VALUES (11, 12, 2, NULL, '2023-04-17 11:28:54', NULL, NULL);
INSERT INTO `userrole` VALUES (12, 13, 2, NULL, '2023-04-17 11:28:58', NULL, NULL);
INSERT INTO `userrole` VALUES (13, 14, 2, NULL, '2023-04-17 11:29:06', NULL, NULL);
INSERT INTO `userrole` VALUES (14, 15, 2, NULL, '2023-04-17 11:29:11', NULL, NULL);
INSERT INTO `userrole` VALUES (15, 16, 2, NULL, '2023-04-17 11:29:17', NULL, NULL);
INSERT INTO `userrole` VALUES (16, 17, 2, NULL, '2023-04-17 11:29:22', NULL, NULL);
INSERT INTO `userrole` VALUES (17, 18, 2, NULL, '2023-04-17 11:29:27', NULL, NULL);
INSERT INTO `userrole` VALUES (18, 26, 2, NULL, '2023-04-20 10:05:45', NULL, NULL);
INSERT INTO `userrole` VALUES (24, 27, 2, NULL, '2023-04-20 15:49:50', NULL, NULL);
INSERT INTO `userrole` VALUES (25, 6, 1, '', '2023-04-20 15:59:08', '', '2023-04-20 15:59:08');

-- ----------------------------
-- Procedure structure for GetChildNode
-- ----------------------------
DROP PROCEDURE IF EXISTS `GetChildNode`;
delimiter ;;
CREATE PROCEDURE `GetChildNode`(IN `pId` BIGINT)
BEGIN

DECLARE stopflag TINYINT DEFAULT FALSE;
DECLARE childId BIGINT DEFAULt 0;

DECLARE childId_cur CURSOR for SELECT id FROM
  (
    SELECT * FROM menus where parentId > 0 ORDER BY parentId, id DESC
  ) realname_sorted,
  (SELECT @pv :=pId) initialisation
  WHERE (FIND_IN_SET(parentId,@pv)>0 And @pv := concat(@pv, ',', id));

DECLARE CONTINUE HANDLER FOR NOT FOUND SET stopflag=TRUE;

open childId_cur;

DROP TABLE IF EXISTS TEMP_T3;
CREATE TEMPORARY TABLE TEMP_T3(childId BIGINT);

FETCH childId_cur into childId;

while(!stopflag) do
BEGIN
#查询到的数据插入临时表，这样才能select出全部
INSERT INTO TEMP_T3(childId) VALUES(childId);
FETCH childId_cur into childId;

END;
END WHILE;
CLOSE childId_cur;
SELECT * FROM TEMP_T3;

END
;;
delimiter ;

SET FOREIGN_KEY_CHECKS = 1;
