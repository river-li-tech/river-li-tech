@ECHO OFF

SET PostDir=%cd%/export/
SET SiteDir=%cd%/
SET PostFormater=%SiteDir%/tools/SiteTools/bin/PostFormater.exe

REM ---------------------------------------------------------
SET PostFile=笔记：MySQL技术内幕-03
SET PostTitle=笔记：MySQL技术内幕-03
SET PostDate=2020-04-14
SET PostCategory=笔记
SET PostTags=数据库
SET PostShortName=innodb_inside_03

ECHO Format post %PostFile%
%PostFormater% %SiteDir% %PostDir% %PostFile% %PostTitle% %PostDate% %PostCategory% %PostTags% %PostShortName%


REM ---------------------------------------------------------
SET PostFile=笔记：MySQL技术内幕-04
SET PostTitle=笔记：MySQL技术内幕-04
SET PostDate=2020-04-15
SET PostCategory=笔记
SET PostTags=数据库
SET PostShortName=innodb_inside_04

ECHO Format post %PostFile%
%PostFormater% %SiteDir% %PostDir% %PostFile% %PostTitle% %PostDate% %PostCategory% %PostTags% %PostShortName%


REM ---------------------------------------------------------
SET PostFile=笔记：MySQL技术内幕-05
SET PostTitle=笔记：MySQL技术内幕-05
SET PostDate=2020-04-16
SET PostCategory=笔记
SET PostTags=数据库
SET PostShortName=innodb_inside_05

ECHO Format post %PostFile%
%PostFormater% %SiteDir% %PostDir% %PostFile% %PostTitle% %PostDate% %PostCategory% %PostTags% %PostShortName%


REM ---------------------------------------------------------
SET PostFile=笔记：MySQL技术内幕-07
SET PostTitle=笔记：MySQL技术内幕-07
SET PostDate=2020-04-17
SET PostCategory=笔记
SET PostTags=数据库
SET PostShortName=innodb_inside_07

ECHO Format post %PostFile%
%PostFormater% %SiteDir% %PostDir% %PostFile% %PostTitle% %PostDate% %PostCategory% %PostTags% %PostShortName%


PAUSE