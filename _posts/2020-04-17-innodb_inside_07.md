---
title: 笔记：MySQL技术内幕-07
date: 2020-04-17
categories: 
  - 笔记
tags: 
  - 数据库
---

<html>
<head>
  <title></title>
  <basefont face="微软雅黑" size="2" />
  <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
  <meta name="exporter-version" content="YXBJ Windows/604762 (zh-CN, DDL); Windows/10.0.0 (Win64); EDAMVersion=V2;"/>
  <style>
    body, td {
      font-family: 微软雅黑;
      font-size: 10pt;
    }
  </style>
</head>
<body>
<a name="10198"/>

<div>
<span><div><div><div><div><br/></div><hr/><div><span style="font-size: unset; color: unset; font-family: unset;">概述</span></div><div>1.数据库引入事务的目的：确保数据库从一种一致状态转换为另一种一致状态</div><div>2.事务须符合ACID特性</div><div>3.Innodb默认事务隔离级别为 READ REPEATABLE</div><div><br/></div><div><br/></div><hr/><div>ACID特征</div><div>1.Atomicity 原子性：事务要么完整执行，要么完全回退到执行前状态</div><div>2.Consistency 一致性：事务执行前后，数据库的<span style="background-color: rgb(255, 250, 165);-evernote-highlight:true;">完整性约束</span>没有破坏</div><div>3.Isolation 隔离性：各个事务操作对象相互隔离，不可见</div><div>    通过锁来实现，也称为<span style="font-size: unset; color: unset; font-family: unset;">并发控制、可串行化等</span></div><div>4.Durability 持久性：事务一旦提交，结果是永久的，高可靠性</div><div><br/></div><div><br/></div><hr/><div>redo日志</div><div>1.用来保证事务的原子性和持久性</div><div>2.由2部分组成：内存中的重做日志缓存（易失的）；重做日志文件（持久的）</div><div>3.Force Log at Commit：事务提交时，所有日志写入，待事务的commit操作完成才算完成；</div><div>4.redo日志用来保证事务的持久性</div><div>   undo日志用来帮助事务回滚及MVCC功能</div><div>5.磁盘的性能决定了事务提交的性能</div><div>   没有使用O_DIRECT选项，因此先写入文件系统缓存，需要fsync刷新</div><div>   允许用户手动设置等待一段时间再执行fsync操作</div><div>6.参数innodb_flush_log_at_trx_commit控制重做日志刷新到磁盘的策略</div><div>   1 事务提交时必须调用一次fsync操作</div><div>   0 master thread每1秒调用一次fsync操作</div><div>   2 写入文件系统缓存中</div><div style="box-sizing: border-box; padding: 8px; font-family: Monaco, Menlo, Consolas, &quot;Courier New&quot;, monospace; font-size: 12px; color: rgb(51, 51, 51); border-radius: 4px; background-color: rgb(251, 250, 248); border: 1px solid rgba(0, 0, 0, 0.15);-en-codeblock:true;"><div>mysql&gt; show variables like '%flush_log_at_trx%';</div><div>+--------------------------------+-------+</div><div>| Variable_name                  | Value |</div><div>+--------------------------------+-------+</div><div>| innodb_flush_log_at_trx_commit | 1     |</div><div>+--------------------------------+-------+</div></div><div>7.binlog 二进制日志</div><div>   重做日志：Innodb存储引擎层产生，物理格式日志，记录的是页的修改，多条事务并发执行时，页操作的日志持续写入（无序的）</div><div>   二进制日志：MySQL数据上层产生，一种逻辑日志，记录的是对应的SQL语句，事务提交完成后写入</div><div>8.重做日志存储方式</div><div>   以512字节存储，大小和磁盘扇区大小一致</div><div>   重做日志块 log block 512B = log header 12B + log tail 8B + log content 492B</div><div>   <img src="https://river-li-tech.github.io/river-li-tech/assets/images/innodb_inside_07/Image.png" type="image/png" data-filename="Image.png" width="577"/></div><div>     log buffer根据一定的规则将内存中的log block刷新到磁盘，具体的规则是</div><div style="box-sizing: border-box; padding: 8px; font-family: Monaco, Menlo, Consolas, &quot;Courier New&quot;, monospace; font-size: 12px; color: rgb(51, 51, 51); border-radius: 4px; background-color: rgb(251, 250, 248); border: 1px solid rgba(0, 0, 0, 0.15);-en-codeblock:true;"><div>1.事务提交时</div><div>2.当log buffer中有一半的内存空间已经被使用时</div><div>3.log checkpoint时</div></div><div>9.Innodb中有51中重做日志类型</div><div>   LSN：8字节，单调递增</div><div>   重做日志记录的是每个页的日志，页中的LSN用来判断页是否需要进行恢复操作</div><div><br/></div><div><br/></div><hr/><div><span style="font-weight: bold;">undo</span></div><div>1.undo日志存放在数据库内部的undo段 segment，位于共享空间内</div><div>2.undo日志是逻辑日志，只是将数据库逻辑地恢复到原来的样子，并发时可能与回滚前后不同</div><div>3.undo日志实现MVVC，当读取的行记录被其它事务占用，则当前事务可通过undo读取之前的行信息，实现非锁定读取</div><div><br/></div><div><br/></div><hr/><div>purge操作</div><div>1.用来最终完成delete和update操作</div><div>2.history list表示按照事务提交的顺序将undo log进行组织</div><div><img src="https://river-li-tech.github.io/river-li-tech/assets/images/innodb_inside_07/Image-1.png" type="image/png" data-filename="Image.png" width="606"/></div><div>  </div><div><br/></div><hr/><div><span style="font-weight: bold;">事务语句</span></div><div>1.一条语句失败并抛出异常时，并不会导致先前已经执行的语句自动回滚</div><div>  <span style="background-color: rgb(255, 250, 165);-evernote-highlight:true;">需要用户显式地执行COMMIT和ROLLBACK命令</span></div><div><img src="https://river-li-tech.github.io/river-li-tech/assets/images/innodb_inside_07/Image-2.png" type="image/png" data-filename="Image.png" width="451"/></div><div><br/></div><div>2.ROLLBACK TO SAVEPOING并不真正地结束一个事务，需要显式地运行<span style="background-color: rgb(255, 250, 165);-evernote-highlight:true;">执行COMMIT和ROLLBACK命令</span></div><div><br/></div><div><br/></div><div><br/></div><div><br/></div><div><br/></div><div><br/></div><div><br/></div><div><br/></div><div><br/></div><div><br/></div><div><br/></div><div><br/></div><div><br/></div><div><br/></div><div><br/></div><div><br/></div><div><br/></div><div><br/></div></div><hr/><div><br/></div></div><hr/><div><br/></div></div><div><br/></div></span>
</div></body></html> 
