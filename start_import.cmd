@ECHO OFF

SET PostDir=%cd%/export/
SET SiteDir=%cd%/
SET PostFormater=%SiteDir%/tools/SiteTools/bin/PostFormater.exe

SET PostFile=�ʼǣ�MySQL������Ļ-04
SET PostTitle=�ʼǣ�MySQL������Ļ-04
SET PostDate=2020-04-14
SET PostCategory=�ʼ�
SET PostTags=���ݿ�
SET PostShortName=innodb_inside_04

ECHO Format post %PostFile%
%PostFormater% %SiteDir% %PostDir% %PostFile% %PostTitle% %PostDate% %PostCategory% %PostTags% %PostShortName%

PAUSE