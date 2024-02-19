USE [BD_Team]
GO
-- SGC: Start Insert Example data
insert into [dbo].[MemberType] ([id], [Description]) values ('E', 'Employee')
insert into [dbo].[MemberType] ([id], [Description]) values ('C', 'Contract')

insert into [dbo].[RoleType] ([id], [Description]) values (1, 'Software Engineer')
insert into [dbo].[RoleType] ([id], [Description]) values (2, 'Project Manager')
insert into [dbo].[RoleType] ([id], [Description]) values (3, 'SCRUM Master')
insert into [dbo].[RoleType] ([id], [Description]) values (4, 'Product Owner')


insert into [dbo].[Tag] ([label]) values ('C#')
insert into [dbo].[Tag] ([label]) values ('Angular')
insert into [dbo].[Tag] ([label]) values ('General Frontend')
insert into [dbo].[Tag] ([label]) values ('Seasoned Leader')
insert into [dbo].[Tag] ([label]) values ('React JS')

insert into [dbo].[Member] ([name],salary_per_year,[type], [role], [country], [currencie_name]) values ('Stan Lee',10000,'C', null, null, null)
insert into [dbo].[Member] ([name],salary_per_year,[type], [role], [country], [currencie_name]) values ('John Doe',5000,'E', 1, 'Brasil','Brazilian real')
insert into [dbo].[Member] ([name],salary_per_year,[type], [role], [country], [currencie_name]) values ('Peter Parker',2000,'C', null, null, null)
insert into [dbo].[Member] ([name],salary_per_year,[type], [role], [country], [currencie_name]) values ('John Snow',1200,'E', 3, 'Chile','Chilean peso')
insert into [dbo].[Member] ([name],salary_per_year,[type], [role], [country], [currencie_name]) values ('Doctor Octopus',10000,'C', null, null, null)

insert into [dbo].[MemberTag] ([member_id],[tag_id]) values (2,1)
insert into [dbo].[MemberTag] ([member_id],[tag_id]) values (2,2)
insert into [dbo].[MemberTag] ([member_id],[tag_id]) values (3,4)

-- SGC: End Example data
-- SGC: Verify inserted data
select * from [dbo].[MemberType]
select * from [dbo].[RoleType]
select * from [dbo].[Tag]
select * from [dbo].[Member]
select * from [dbo].[MemberTag] 