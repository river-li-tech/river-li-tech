@ECHO OFF

SET PostDir=%cd%/export/
SET SiteDir=%cd%/
SET PostFormater=%SiteDir%/tools/SiteTools/bin/PostFormater.exe

SET PostFile=笔记：MySQL技术内幕-04
SET PostTitle=笔记：MySQL技术内幕-04
SET PostDate=2020-04-14
SET PostCategory=笔记
SET PostTags=数据库
SET PostShortName=innodb_inside_04

ECHO Format post %PostFile%
%PostFormater% %SiteDir% %PostDir% %PostFile% %PostTitle% %PostDate% %PostCategory% %PostTags% %PostShortName%

PAUSE