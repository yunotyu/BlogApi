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

 Date: 12/04/2023 15:34:39
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
  `isDel` tinyint(1) NULL DEFAULT 0,
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
  `isDel` tinyint(1) NULL DEFAULT 0,
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
  `isDel` tinyint(1) NULL DEFAULT 0,
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
  `id` bigint(0) NOT NULL AUTO_INCREMENT,
  `menuNames` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `parentId` int(0) NULL DEFAULT NULL COMMENT 'parentId 为0说明是根节点',
  `depth` int(0) NULL DEFAULT NULL COMMENT '根节点下面的最大子节点的长度',
  `url` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '对应的路由',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of menus
-- ----------------------------
INSERT INTO `menus` VALUES (1, '用户管理', 0, NULL, '/user');
INSERT INTO `menus` VALUES (2, '角色管理', 0, NULL, '/role');
INSERT INTO `menus` VALUES (3, '权限管理', 0, NULL, '/permisson');

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
  `isDel` tinyint(1) NULL DEFAULT 0 COMMENT '0:未删除，1:已删除',
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
  `isShow` tinyint(1) NULL DEFAULT 0 COMMENT '是否显示该菜单项，0代表不显示，1代表显示',
  `description` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `enable` tinyint(1) NULL DEFAULT 1 COMMENT '是否启用，1代表启用，0代表不启用',
  `isDel` tinyint(1) NULL DEFAULT 0 COMMENT '是否逻辑删除，0代表未删除，1代表删除\r\n是否逻辑删除，0代表未删除，1代表删除',
  `createName` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `createTime` datetime(0) NULL DEFAULT NULL,
  `modifyName` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '修改人的用户名',
  `modifyTime` datetime(0) NULL DEFAULT NULL COMMENT '修改时间',
  `menuId` bigint(0) NULL DEFAULT NULL COMMENT '菜单id',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `menuIdForKey`(`menuId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of permission
-- ----------------------------
INSERT INTO `permission` VALUES (2, NULL, 0, 1, NULL, 1, 0, 'admin', '2023-04-04 17:29:51', NULL, NULL, 1);
INSERT INTO `permission` VALUES (3, NULL, 0, 0, NULL, 1, 0, 'admin', '2023-04-04 17:32:00', NULL, NULL, 2);
INSERT INTO `permission` VALUES (4, NULL, 0, 0, NULL, 1, 0, 'admin', '2023-04-04 17:32:16', NULL, NULL, 3);

-- ----------------------------
-- Table structure for role
-- ----------------------------
DROP TABLE IF EXISTS `role`;
CREATE TABLE `role`  (
  `id` bigint(0) NOT NULL AUTO_INCREMENT,
  `roleName` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL COMMENT '角色名',
  `description` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '角色描述',
  `level` int(0) NULL DEFAULT NULL COMMENT '角色等级',
  `enable` tinyint(1) NULL DEFAULT 1 COMMENT '是否启用，1代表启用，0代表不启用',
  `isDel` tinyint(1) NULL DEFAULT 0 COMMENT '是否逻辑删除，0代表未删除，1代表删除',
  `createUsername` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL COMMENT '创建的用户名',
  `createTime` datetime(0) NOT NULL COMMENT '创建时间',
  `modifyTime` datetime(0) NULL DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of role
-- ----------------------------
INSERT INTO `role` VALUES (1, '管理员', NULL, NULL, 1, 0, '管理员', '2023-04-04 17:05:16', NULL);

-- ----------------------------
-- Table structure for rolepermission
-- ----------------------------
DROP TABLE IF EXISTS `rolepermission`;
CREATE TABLE `rolepermission`  (
  `id` bigint(0) NOT NULL AUTO_INCREMENT,
  `roleId` bigint(0) NOT NULL COMMENT '外键，角色表',
  `permissionId` bigint(0) NOT NULL COMMENT '外键，permission表',
  `isDel` tinyint(1) NULL DEFAULT 0 COMMENT '是否逻辑删除，0代表未删除，1代表删除',
  `enabel` tinyint(1) NULL DEFAULT 1 COMMENT '是否启用，1代表启用，0代表不启用',
  `createUsername` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `createTime` datetime(0) NULL DEFAULT NULL,
  `modifyUsername` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `modifyTime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `roleForKey`(`roleId`) USING BTREE,
  INDEX `perForKey`(`permissionId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of rolepermission
-- ----------------------------
INSERT INTO `rolepermission` VALUES (1, 1, 2, 0, 1, 'admin', '2023-04-04 17:34:01', NULL, NULL);
INSERT INTO `rolepermission` VALUES (2, 1, 3, 0, 1, 'admin', '2023-04-04 17:34:19', NULL, NULL);
INSERT INTO `rolepermission` VALUES (3, 1, 4, 0, 1, 'admin', '2023-04-04 17:34:39', NULL, NULL);

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
  `isDel` tinyint(1) NULL DEFAULT 0 COMMENT '是否逻辑删除，0代表未删除，1代表删除',
  `isEnable` tinyint(1) NULL DEFAULT 1 COMMENT '是否启用，1代表启用，0代表不启用',
  `modifyName` varchar(25) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL COMMENT '修改信息用户名',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of user
-- ----------------------------
INSERT INTO `user` VALUES (1, 'admin', 'admin', '2023-04-03 17:25:44', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 1, NULL);

-- ----------------------------
-- Table structure for userrole
-- ----------------------------
DROP TABLE IF EXISTS `userrole`;
CREATE TABLE `userrole`  (
  `id` bigint(0) NOT NULL AUTO_INCREMENT,
  `userId` bigint(0) NOT NULL COMMENT '外键，关联user表id',
  `roleId` bigint(0) NOT NULL COMMENT '外键，关联角色表id',
  `isDel` tinyint(1) NULL DEFAULT 0 COMMENT '是否逻辑删除，0代表未删除，1代表删除',
  `enable` tinyint(1) NULL DEFAULT 1 COMMENT '是否启用，1代表启用，0代表不启用',
  `createUsername` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `createTime` datetime(0) NULL DEFAULT NULL,
  `modifyUsername` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NULL DEFAULT NULL,
  `modifyTime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `useridForKey`(`userId`) USING BTREE,
  INDEX `roleidForKey`(`roleId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of userrole
-- ----------------------------
INSERT INTO `userrole` VALUES (1, 1, 1, 0, 1, 'admin', '2023-04-04 17:32:52', NULL, NULL);

SET FOREIGN_KEY_CHECKS = 1;
