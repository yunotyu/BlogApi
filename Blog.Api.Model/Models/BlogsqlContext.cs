using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Model.Models;

public partial class BlogsqlContext : DbContext
{
    public BlogsqlContext()
    {
    }

    public BlogsqlContext(DbContextOptions<BlogsqlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Advertisement> Advertisements { get; set; }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Collection> Collections { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Exceptionlog> Exceptionlogs { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Operatelog> Operatelogs { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Rolepermission> Rolepermissions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userrole> Userroles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=127.0.0.1;user id=root;password=123456;database=blogsql", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.22-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_bin")
            .HasCharSet("utf8");

        modelBuilder.Entity<Advertisement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("advertisement");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("createTime");
            entity.Property(e => e.ImgUrl)
                .HasMaxLength(255)
                .HasComment("广告图片")
                .HasColumnName("imgUrl");
            entity.Property(e => e.IsDel)
                .HasDefaultValueSql("'0'")
                .HasColumnName("isDel");
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .HasColumnName("remark");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasComment("广告标题")
                .HasColumnName("title");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .HasComment("广告链接")
                .HasColumnName("url");
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("article");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArticleContent)
                .HasComment("文章内容，注意长度限制，6000字以下")
                .HasColumnType("varchar(20000)")
                .HasColumnName("articleContent")
                .UseCollation("utf8_general_ci");
            entity.Property(e => e.ArticleKind)
                .HasMaxLength(500)
                .HasComment("文章分类，以--分隔")
                .HasColumnName("articleKind")
                .UseCollation("utf8_general_ci");
            entity.Property(e => e.ArticleTitle)
                .HasMaxLength(255)
                .HasComment("文章标题")
                .HasColumnName("articleTitle")
                .UseCollation("utf8_general_ci");
            entity.Property(e => e.CommentCount)
                .HasComment("评论数")
                .HasColumnName("commentCount");
            entity.Property(e => e.CreateTime)
                .HasComment("文章发布时间")
                .HasColumnType("datetime")
                .HasColumnName("createTime");
            entity.Property(e => e.CreateUsername)
                .HasMaxLength(255)
                .HasComment("发布文章的用户名")
                .HasColumnName("createUsername")
                .UseCollation("utf8_general_ci");
            entity.Property(e => e.IsDel)
                .HasDefaultValueSql("'0'")
                .HasColumnName("isDel");
            entity.Property(e => e.LikeCount)
                .HasComment("点赞数")
                .HasColumnName("likeCount");
            entity.Property(e => e.LookCount)
                .HasComment("文章访问次数")
                .HasColumnName("lookCount");
        });

        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("collection");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArticleId)
                .HasComment("文章id")
                .HasColumnName("articleId");
            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("createTime");
            entity.Property(e => e.CreateUsername)
                .HasMaxLength(255)
                .HasColumnName("createUsername");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("comments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArticleId)
                .HasComment("外键，关联对应的文章")
                .HasColumnName("articleId");
            entity.Property(e => e.Comment1)
                .HasMaxLength(1200)
                .HasComment("评论，500字以内")
                .HasColumnName("comment");
            entity.Property(e => e.CreateName)
                .HasMaxLength(255)
                .HasDefaultValueSql("'游客'")
                .HasComment("可以为空，默认为游客访问")
                .HasColumnName("createName");
            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("createTime");
            entity.Property(e => e.IsDel)
                .HasDefaultValueSql("'0'")
                .HasColumnName("isDel");
        });

        modelBuilder.Entity<Exceptionlog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("exceptionlog");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ExceptionType)
                .HasMaxLength(255)
                .HasComment("异常类型")
                .HasColumnName("exceptionType");
            entity.Property(e => e.Level)
                .HasMaxLength(255)
                .HasComment("异常等级")
                .HasColumnName("level");
            entity.Property(e => e.Message)
                .HasMaxLength(2000)
                .HasComment("异常信息")
                .HasColumnName("message");
            entity.Property(e => e.ThreadId)
                .HasComment("异常线程id")
                .HasColumnName("threadId");
            entity.Property(e => e.Time)
                .HasColumnType("datetime")
                .HasColumnName("time");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("menus");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Depth)
                .HasComment("根节点下面的最大子节点的长度")
                .HasColumnName("depth");
            entity.Property(e => e.MenuNames)
                .HasMaxLength(255)
                .HasColumnName("menuNames");
            entity.Property(e => e.ParentId)
                .HasComment("parentId 为0说明是根节点")
                .HasColumnName("parentId");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .HasComment("对应的路由")
                .HasColumnName("url");
        });

        modelBuilder.Entity<Operatelog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("operatelog");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActionName)
                .HasMaxLength(255)
                .HasComment("访问的action方法")
                .HasColumnName("actionName");
            entity.Property(e => e.Area)
                .HasMaxLength(255)
                .HasComment("访问的区域")
                .HasColumnName("area");
            entity.Property(e => e.ControllerName)
                .HasMaxLength(255)
                .HasComment("访问的控制器名")
                .HasColumnName("controllerName");
            entity.Property(e => e.Ip)
                .HasMaxLength(255)
                .HasComment("来访的ip")
                .HasColumnName("ip");
            entity.Property(e => e.IsDel)
                .HasDefaultValueSql("'0'")
                .HasComment("0:未删除，1:已删除")
                .HasColumnName("isDel");
            entity.Property(e => e.LogTime)
                .HasComment("访问的时间")
                .HasColumnType("datetime")
                .HasColumnName("logTime");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasComment("可以为空，如果是没登录的用户")
                .HasColumnName("username");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("permission");

            entity.HasIndex(e => e.MenuId, "menuIdForKey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateName)
                .HasMaxLength(255)
                .HasColumnName("createName");
            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("createTime");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Enable)
                .HasDefaultValueSql("'1'")
                .HasComment("是否启用，1代表启用，0代表不启用")
                .HasColumnName("enable");
            entity.Property(e => e.Icon)
                .HasMaxLength(1000)
                .HasComment("菜单图标")
                .HasColumnName("icon");
            entity.Property(e => e.IsBtn)
                .HasDefaultValueSql("'0'")
                .HasComment("是否显示菜单为按钮，0代表不显示，1显示")
                .HasColumnName("isBtn");
            entity.Property(e => e.IsDel)
                .HasDefaultValueSql("'0'")
                .HasComment("是否逻辑删除，0代表未删除，1代表删除\r\n是否逻辑删除，0代表未删除，1代表删除")
                .HasColumnName("isDel");
            entity.Property(e => e.IsShow)
                .HasDefaultValueSql("'0'")
                .HasComment("是否显示该菜单项，0代表不显示，1代表显示")
                .HasColumnName("isShow");
            entity.Property(e => e.MenuId)
                .HasComment("菜单id")
                .HasColumnName("menuId");
            entity.Property(e => e.ModifyName)
                .HasMaxLength(255)
                .HasComment("修改人的用户名")
                .HasColumnName("modifyName");
            entity.Property(e => e.ModifyTime)
                .HasComment("修改时间")
                .HasColumnType("datetime")
                .HasColumnName("modifyTime");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasComment("创建时间")
                .HasColumnType("datetime")
                .HasColumnName("createTime");
            entity.Property(e => e.CreateUsername)
                .HasMaxLength(255)
                .HasComment("创建的用户名")
                .HasColumnName("createUsername");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasComment("角色描述")
                .HasColumnName("description");
            entity.Property(e => e.Enable)
                .HasDefaultValueSql("'1'")
                .HasComment("是否启用，1代表启用，0代表不启用")
                .HasColumnName("enable");
            entity.Property(e => e.IsDel)
                .HasDefaultValueSql("'0'")
                .HasComment("是否逻辑删除，0代表未删除，1代表删除")
                .HasColumnName("isDel");
            entity.Property(e => e.Level)
                .HasComment("角色等级")
                .HasColumnName("level");
            entity.Property(e => e.ModifyTime)
                .HasComment("修改时间")
                .HasColumnType("datetime")
                .HasColumnName("modifyTime");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .HasComment("角色名")
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<Rolepermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("rolepermission");

            entity.HasIndex(e => e.PermissionId, "perForKey");

            entity.HasIndex(e => e.RoleId, "roleForKey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("createTime");
            entity.Property(e => e.CreateUsername)
                .HasMaxLength(255)
                .HasColumnName("createUsername");
            entity.Property(e => e.Enabel)
                .HasDefaultValueSql("'1'")
                .HasComment("是否启用，1代表启用，0代表不启用")
                .HasColumnName("enabel");
            entity.Property(e => e.IsDel)
                .HasDefaultValueSql("'0'")
                .HasComment("是否逻辑删除，0代表未删除，1代表删除")
                .HasColumnName("isDel");
            entity.Property(e => e.ModifyTime)
                .HasColumnType("datetime")
                .HasColumnName("modifyTime");
            entity.Property(e => e.ModifyUsername)
                .HasMaxLength(255)
                .HasColumnName("modifyUsername");
            entity.Property(e => e.PermissionId)
                .HasComment("外键，permission表")
                .HasColumnName("permissionId");
            entity.Property(e => e.RoleId)
                .HasComment("外键，角色表")
                .HasColumnName("roleId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Age)
                .HasComment("年龄")
                .HasColumnName("age");
            entity.Property(e => e.Birth)
                .HasMaxLength(6)
                .HasComment("生日")
                .HasColumnName("birth");
            entity.Property(e => e.CreateTime)
                .HasComment("创建时间")
                .HasColumnType("datetime")
                .HasColumnName("createTime");
            entity.Property(e => e.ErrorCount)
                .HasComment("密码错误次数")
                .HasColumnName("errorCount");
            entity.Property(e => e.IsDel)
                .HasDefaultValueSql("'0'")
                .HasComment("是否逻辑删除，0代表未删除，1代表删除")
                .HasColumnName("isDel");
            entity.Property(e => e.IsEnable)
                .HasDefaultValueSql("'1'")
                .HasComment("是否启用，1代表启用，0代表不启用")
                .HasColumnName("isEnable");
            entity.Property(e => e.LastErrorTime)
                .HasComment("上次密码错误时间")
                .HasColumnType("datetime")
                .HasColumnName("lastErrorTime");
            entity.Property(e => e.ModifyName)
                .HasMaxLength(25)
                .HasComment("修改信息用户名")
                .HasColumnName("modifyName");
            entity.Property(e => e.Pwd)
                .HasMaxLength(255)
                .HasColumnName("pwd");
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .HasComment("备注")
                .HasColumnName("remark");
            entity.Property(e => e.Sex)
                .HasComment("性别 0:男  1女")
                .HasColumnName("sex");
            entity.Property(e => e.UpdateTime)
                .HasComment("修改信息时间")
                .HasColumnType("datetime")
                .HasColumnName("updateTime");
            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Userrole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("userrole");

            entity.HasIndex(e => e.RoleId, "roleidForKey");

            entity.HasIndex(e => e.UserId, "useridForKey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("createTime");
            entity.Property(e => e.CreateUsername)
                .HasMaxLength(255)
                .HasColumnName("createUsername");
            entity.Property(e => e.Enable)
                .HasDefaultValueSql("'1'")
                .HasComment("是否启用，1代表启用，0代表不启用")
                .HasColumnName("enable");
            entity.Property(e => e.IsDel)
                .HasDefaultValueSql("'0'")
                .HasComment("是否逻辑删除，0代表未删除，1代表删除")
                .HasColumnName("isDel");
            entity.Property(e => e.ModifyTime)
                .HasColumnType("datetime")
                .HasColumnName("modifyTime");
            entity.Property(e => e.ModifyUsername)
                .HasMaxLength(255)
                .HasColumnName("modifyUsername");
            entity.Property(e => e.RoleId)
                .HasComment("外键，关联角色表id")
                .HasColumnName("roleId");
            entity.Property(e => e.UserId)
                .HasComment("外键，关联user表id")
                .HasColumnName("userId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
