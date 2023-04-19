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

 Date: 19/04/2023 16:18:33
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
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

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
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

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
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

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
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

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
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for menus
-- ----------------------------
DROP TABLE IF EXISTS `menus`;
CREATE TABLE `menus`  (
  `id` bigint(0) NOT NULL AUTO_INCREMENT,
  `menuNames` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `parentId` int(0) NULL DEFAULT NULL COMMENT 'parentId 为0说明是根节点',
  `depth` int(0) NULL DEFAULT NULL COMMENT '根节点下面的最大子节点的长度',
  `url` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL COMMENT '对应的路由',
  `isDel` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 7 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of menus
-- ----------------------------
INSERT INTO `menus` VALUES (1, '用户管理', 0, NULL, '/user', 0);
INSERT INTO `menus` VALUES (2, '角色管理', 0, NULL, '/role', 0);
INSERT INTO `menus` VALUES (3, '权限管理', 0, NULL, '/permisson', 0);
INSERT INTO `menus` VALUES (6, '主界面', 0, NULL, '/home', 0);

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
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

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
  `isDel` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否逻辑删除，0代表未删除，1代表删除\r\n是否逻辑删除，0代表未删除，1代表删除',
  `createName` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `createTime` datetime(0) NULL DEFAULT NULL,
  `modifyName` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '修改人的用户名',
  `modifyTime` datetime(0) NULL DEFAULT NULL COMMENT '修改时间',
  `menuId` bigint(0) NOT NULL COMMENT '菜单id',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `menuIdForKey`(`menuId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of permission
-- ----------------------------
INSERT INTO `permission` VALUES (2, NULL, 0, 1, NULL, 0, 'admin', '2023-04-04 17:29:51', NULL, NULL, 1);
INSERT INTO `permission` VALUES (3, NULL, 0, 0, NULL, 0, 'admin', '2023-04-04 17:32:00', NULL, NULL, 2);
INSERT INTO `permission` VALUES (4, NULL, 0, 0, NULL, 0, 'admin', '2023-04-04 17:32:16', NULL, NULL, 3);
INSERT INTO `permission` VALUES (5, NULL, 0, 0, NULL, 0, 'admin', '2023-04-13 13:44:52', NULL, NULL, 6);

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
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of role
-- ----------------------------
INSERT INTO `role` VALUES (1, 'admin', '管理员', NULL, 1, 0, '管理员', '2023-04-04 17:05:16', NULL);
INSERT INTO `role` VALUES (2, 'user', '普通用户', NULL, 1, 0, '管理员', '2023-04-12 15:43:33', NULL);

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
) ENGINE = InnoDB AUTO_INCREMENT = 23 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

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
INSERT INTO `user` VALUES (19, 'yu', '202CB962AC59075B964B07152D234B70', '0001-01-01 00:00:00', NULL, NULL, NULL, 'ggg', 1, 20, '2023-04-17 15:25:38.298000', 0, 1, 'yu');
INSERT INTO `user` VALUES (20, 'yu', '202CB962AC59075B964B07152D234B70', '0001-01-01 00:00:00', NULL, NULL, NULL, 'ggg', 1, 20, '2023-04-17 15:25:38.298000', 0, 1, 'yu');
INSERT INTO `user` VALUES (21, 'yu', '202CB962AC59075B964B07152D234B70', '0001-01-01 00:00:00', NULL, NULL, NULL, 'ggg', 1, 20, '2023-04-17 15:25:38.298000', 0, 1, '');
INSERT INTO `user` VALUES (22, 'yu', '202CB962AC59075B964B07152D234B70', '0001-01-01 00:00:00', NULL, NULL, NULL, 'ggg', 1, 20, '2023-04-17 15:25:38.298000', 0, 1, '');

-- ----------------------------
-- Table structure for userrole
-- ----------------------------
DROP TABLE IF EXISTS `userrole`;
CREATE TABLE `userrole`  (
  `id` bigint(0) NOT NULL AUTO_INCREMENT,
  `userId` bigint(0) NOT NULL COMMENT '外键，关联user表id',
  `roleId` bigint(0) NOT NULL COMMENT '外键，关联角色表id',
  `isDel` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否逻辑删除，0代表未删除，1代表删除',
  `createUsername` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `createTime` datetime(0) NULL DEFAULT NULL,
  `modifyUsername` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `modifyTime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `useridForKey`(`userId`) USING BTREE,
  INDEX `roleidForKey`(`roleId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 18 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of userrole
-- ----------------------------
INSERT INTO `userrole` VALUES (5, 5, 1, 0, NULL, '2023-04-13 10:19:26', NULL, NULL);
INSERT INTO `userrole` VALUES (6, 6, 1, 0, NULL, '2023-04-13 10:57:35', NULL, NULL);
INSERT INTO `userrole` VALUES (7, 5, 2, 0, NULL, '2023-04-13 11:09:18', NULL, NULL);
INSERT INTO `userrole` VALUES (8, 9, 2, 0, NULL, '2023-04-17 11:28:21', NULL, NULL);
INSERT INTO `userrole` VALUES (9, 10, 2, 0, NULL, '2023-04-17 11:28:33', NULL, NULL);
INSERT INTO `userrole` VALUES (10, 11, 2, 0, NULL, '2023-04-17 11:28:51', NULL, NULL);
INSERT INTO `userrole` VALUES (11, 12, 2, 0, NULL, '2023-04-17 11:28:54', NULL, NULL);
INSERT INTO `userrole` VALUES (12, 13, 2, 0, NULL, '2023-04-17 11:28:58', NULL, NULL);
INSERT INTO `userrole` VALUES (13, 14, 2, 0, NULL, '2023-04-17 11:29:06', NULL, NULL);
INSERT INTO `userrole` VALUES (14, 15, 2, 0, NULL, '2023-04-17 11:29:11', NULL, NULL);
INSERT INTO `userrole` VALUES (15, 16, 2, 0, NULL, '2023-04-17 11:29:17', NULL, NULL);
INSERT INTO `userrole` VALUES (16, 17, 2, 0, NULL, '2023-04-17 11:29:22', NULL, NULL);
INSERT INTO `userrole` VALUES (17, 18, 2, 0, NULL, '2023-04-17 11:29:27', NULL, NULL);

-- ----------------------------
-- Table structure for usertemp5558ccc4
-- ----------------------------
DROP TABLE IF EXISTS `usertemp5558ccc4`;
CREATE TABLE `usertemp5558ccc4`  (
  `id` bigint(0) NOT NULL DEFAULT 0,
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
  `modifyName` varchar(25) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '修改信息用户名'
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
