import Task, { TaskStatus } from "../../models/task";
import { mockedProject1, mockedProject2, mockedProject3, mockedProject4 } from "./project";
import { mockedUserMain, mockedUser2, mockedUser3 } from "./user";


export const mockedTasks = [
    {
        id: 1,
        name: "Update website content",
        description: "Review and update the content on the company website to reflect recent changes and improvements.",
        status: TaskStatus.TODO,
        userId: mockedUserMain.id,
        projectId: mockedProject1.id
    },
    {
        id: 2,
        name: "Prepare quarterly financial report",
        description: "Compile financial data and create a comprehensive report for the last quarter\"s performance.",
        status: TaskStatus.IN_PROGRESS,
        userId: mockedUserMain.id,
        projectId: mockedProject2.id
    },
    {
        id: 3,
        name: "Conduct market research",
        description: "Perform market analysis to identify potential target markets and competitors for a new product launch.",
        status: TaskStatus.TODO,
        userId: mockedUser3.id,
        projectId: mockedProject3.id
    },
    {
        id: 4,
        name: "Develop mobile app interface",
        description: "Design and develop the user interface for the upcoming mobile application.",
        status: TaskStatus.DONE,
        userId: mockedUser2.id,
        projectId: mockedProject4.id
    },
    {
        id: 5,
        name: "Test software for bugs",
        description: "Execute various test cases to identify and report any bugs or issues in the software system.",
        status: TaskStatus.IN_PROGRESS,
        userId: mockedUser2.id,
        projectId: mockedProject1.id
    },
    {
        id: 6,
        name: "Create social media content calendar",
        description: "Plan and schedule social media posts for the upcoming month, aligning with marketing objectives.",
        status: TaskStatus.DONE,
        userId: mockedUserMain.id,
        projectId: mockedProject2.id
    },
    {
        id: 7,
        name: "Conduct employee training session",
        description: "Organize and deliver a training session for employees to enhance their skills and knowledge.",
        status: TaskStatus.TODO,
        userId: mockedUser3.id,
        projectId: mockedProject3.id
    },
    {
        id: 8,
        name: "Implement SEO strategies",
        description: "Optimize the website for search engines by implementing effective SEO techniques.",
        status: TaskStatus.IN_PROGRESS,
        userId: mockedUser2.id,
        projectId: mockedProject4.id
    },
    {
        id: 9,
        name: "Design product packaging",
        description: "Create an appealing and informative packaging design for the new product line.",
        status: TaskStatus.DONE,
        userId: mockedUser3.id,
        projectId: mockedProject1.id
    },
    {
        id: 10,
        name: "Conduct customer satisfaction survey",
        description: "Collect feedback from customers through a survey to assess their satisfaction levels and identify areas for improvement.",
        status: TaskStatus.DONE,
        userId: mockedUserMain.id,
        projectId: mockedProject2.id
    },
    {
        id: 11,
        name: "Update inventory records",
        description: "Audit and update the inventory records to ensure accuracy and facilitate efficient stock management.",
        status: TaskStatus.TODO,
        userId: mockedUser2.id,
        projectId: mockedProject3.id
    },
    {
        id: 12,
        name: "Develop marketing campaign strategy",
        description: "Formulate a comprehensive marketing campaign strategy to promote a new product launch.",
        status: TaskStatus.IN_PROGRESS,
        userId: mockedUser3.id,
        projectId: mockedProject4.id
    },
    {
        id: 13,
        name: "Conduct competitor analysis",
        description: "Research and analyze the strategies and offerings of key competitors in the industry.",
        status: TaskStatus.DONE,
        userId: mockedUserMain.id,
        projectId: mockedProject1.id
    },
    {
        id: 14,
        name: "Enhance user interface design",
        description: "Improve the user interface design of the web application to enhance user experience and usability.",
        status: TaskStatus.IN_PROGRESS,
        userId: mockedUser2.id,
        projectId: mockedProject2.id
    },
    {
        id: 15,
        name: "Prepare sales presentation",
        description: "Create a compelling sales presentation to showcase the product features and benefits to potential clients.",
        status: TaskStatus.DONE,
        userId: mockedUser3.id,
        projectId: mockedProject3.id
    },
    {
        id: 16,
        name: "Conduct usability testing",
        description: "Test the user-friendliness and effectiveness of the software application through usability testing sessions.",
        status: TaskStatus.TODO,
        userId: mockedUser2.id,
        projectId: mockedProject4.id
    },
    {
        id: 17,
        name: "Create video tutorial series",
        description: "Produce a series of instructional videos to guide users in using the software product.",
        status: TaskStatus.IN_PROGRESS,
        userId: mockedUser2.id,
        projectId: mockedProject1.id
    },
    {
        id: 18,
        name: "Optimize database performance",
        description: "Analyze and fine-tune the database queries and configurations for improved performance.",
        status: TaskStatus.TODO,
        userId: mockedUserMain.id,
        projectId: mockedProject2.id
    },
    {
        id: 19,
        name: "Develop e-commerce payment integration",
        description: "Integrate secure payment gateways into the e-commerce platform for seamless transactions.",
        status: TaskStatus.IN_PROGRESS,
        userId: mockedUser3.id,
        projectId: mockedProject3.id
    },
    {
        id: 20,
        name: "Create user documentation",
        description: "Write comprehensive documentation to guide users in using the software product effectively.",
        status: TaskStatus.DONE,
        userId: mockedUser2.id,
        projectId: mockedProject4.id
    },
    {
        id: 21,
        name: "Conduct A/B testing for website",
        description: "Set up and run A/B tests to compare different website variations and analyze user engagement metrics.",
        status: "IN_PROGRESS",
        userId: mockedUser3.id,
        projectId: mockedProject1.id
    },
    {
        id: 22,
        name: "Develop cross-platform mobile app",
        description: "Build a mobile application that runs on both iOS and Android platforms using cross-platform frameworks.",
        status: TaskStatus.TODO,
        userId: mockedUserMain.id,
        projectId: mockedProject2.id
    },
    {
        id: 23,
        name: "Create marketing collateral",
        description: "Design and produce marketing materials such as brochures, flyers, and banners for promotional campaigns.",
        status: TaskStatus.DONE,
        userId: mockedUser2.id,
        projectId: mockedProject3.id
    },
    {
        id: 24,
        name: "Implement security enhancements",
        description: "Strengthen the system\"s security by implementing additional security measures and best practices.",
        status: TaskStatus.IN_PROGRESS,
        userId: mockedUser3.id,
        projectId: mockedProject4.id
    },
    {
        id: 25,
        name: "Conduct user interviews",
        description: "Interview users to gather feedback and insights on their experiences and expectations for the product.",
        status: TaskStatus.DONE,
        userId: mockedUserMain.id,
        projectId: mockedProject1.id
    },
    {
        id: 26,
        name: "Perform data analysis",
        description: "Analyze large datasets to identify patterns, trends, and actionable insights for decision-making.",
        status: TaskStatus.TODO,
        userId: mockedUser2.id,
        projectId: mockedProject2.id
    },
    {
        id: 27,
        name: "Set up email marketing campaign",
        description: "Design email templates and configure an automated email marketing campaign to engage with customers.",
        status: TaskStatus.IN_PROGRESS,
        userId: mockedUser3.id,
        projectId: mockedProject3.id
    },
    {
        id: 28,
        name: "Optimize website loading speed",
        description: "Optimize website performance by reducing page load times and optimizing server-side configurations.",
        status: TaskStatus.DONE,
        userId: mockedUser2.id,
        projectId: mockedProject4.id
    },
    {
        id: 29,
        name: "Create interactive data visualization",
        description: "Develop interactive charts and visualizations to present complex data in an easily understandable format.",
        status: TaskStatus.IN_PROGRESS,
        userId: mockedUserMain.id,
        projectId: mockedProject1.id
    },
    {
        id: 30,
        name: "Conduct employee performance reviews",
        description: "Assess employee performance and provide feedback through structured performance review sessions.",
        status: TaskStatus.TODO,
        userId: mockedUser3.id,
        projectId: mockedProject2.id
    },
    {
        id: 31,
        name: "Develop API documentation",
        description: "Create comprehensive documentation for the API, including endpoints, parameters, and response formats.",
        status: TaskStatus.DONE,
        userId: mockedUser2.id,
        projectId: mockedProject3.id
    },
    {
        id: 32,
        name: "Create automated test scripts",
        description: "Develop automated test scripts to ensure software quality and reduce manual testing efforts.",
        status: TaskStatus.IN_PROGRESS,
        userId: mockedUser2.id,
        projectId: mockedProject4.id
    },
    {
        id: 33,
        name: "Plan and execute online advertising campaign",
        description: "Design and run online advertising campaigns on various platforms to increase brand visibility and reach.",
        status: TaskStatus.TODO,
        userId: mockedUser3.id,
        projectId: mockedProject1.id
    },
    {
        id: 34,
        name: "Implement responsive design for website",
        description: "Optimize the website layout and styles to ensure seamless user experience across different devices and screen sizes.",
        status: TaskStatus.IN_PROGRESS,
        userId: mockedUserMain.id,
        projectId: mockedProject2.id
    },
    {
        id: 35,
        name: "Conduct user acceptance testing",
        description: "Engage end-users to test the software application and gather feedback on usability and functionality.",
        status: TaskStatus.DONE,
        userId: mockedUser2.id,
        projectId: mockedProject3.id
    },
    {
        id: 36,
        name: "Create project timeline and milestones",
        description: "Develop a project timeline with clear milestones and deadlines to ensure timely project completion.",
        status: TaskStatus.TODO,
        userId: mockedUser3.id,
        projectId: mockedProject4.id
    },
    {
        id: 37,
        name: "Provide technical support to customers",
        description: "Assist customers in troubleshooting technical issues and provide solutions and guidance.",
        status: TaskStatus.IN_PROGRESS,
        userId: mockedUserMain.id,
        projectId: mockedProject1.id
    },
    {
        id: 38,
        name: "Conduct usability testing",
        description: "Test the user-friendliness and effectiveness of the software application through usability testing sessions.",
        status: TaskStatus.DONE,
        userId: mockedUser2.id,
        projectId: mockedProject2.id
    },
    {
        id: 39,
        name: "Develop online booking system",
        description: "Build an online booking system to facilitate seamless appointment scheduling and management.",
        status: TaskStatus.IN_PROGRESS,
        userId: mockedUser3.id,
        projectId: mockedProject3.id
    },
    {
        id: 40,
        name: "Conduct customer feedback survey",
        description: "Design and administer a survey to gather customer feedback on product satisfaction and preferences.",
        status: TaskStatus.DONE,
        userId: mockedUser2.id,
        projectId: mockedProject4.id
    },
    {
        id: 41,
        name: "Develop content marketing strategy",
        description: "Create a comprehensive strategy for content marketing, including content creation, distribution, and engagement.",
        status: TaskStatus.TODO,
        userId: mockedUser2.id,
        projectId: mockedProject1.id
    },
    {
        id: 42,
        name: "Integrate third-party payment gateway",
        description: "Integrate a secure and reliable third-party payment gateway to process online transactions.",
        status: TaskStatus.IN_PROGRESS,
        userId: mockedUser3.id,
        projectId: mockedProject2.id
    },
    {
        id: 43,
        name: "Perform data migration to new system",
        description: "Migrate data from the existing system to a new system while ensuring data integrity and minimal downtime.",
        status: TaskStatus.TODO,
        userId: mockedUser2.id,
        projectId: mockedProject3.id
    },
    {
        id: 44,
        name: "Design and implement gamification elements",
        description: "Incorporate gamification elements into the software product to enhance user engagement and motivation.",
        status: TaskStatus.IN_PROGRESS,
        userId: mockedUser2.id,
        projectId: mockedProject4.id
    },
    {
        id: 45,
        name: "Conduct employee training program",
        description: "Plan and deliver a training program to enhance employees\" skills and knowledge in specific areas.",
        status: TaskStatus.DONE,
        userId: mockedUser3.id,
        projectId: mockedProject1.id
    },
    {
        id: 46,
        name: "Optimize website for search engines (SEO)",
        description: "Implement SEO techniques and optimizations to improve the website\"s visibility and search engine rankings.",
        status: TaskStatus.IN_PROGRESS,
        userId: mockedUserMain.id,
        projectId: mockedProject2.id
    },
    {
        id: 47,
        name: "Create social media advertising campaign",
        description: "Develop and launch targeted advertising campaigns on social media platforms to reach the desired audience.",
        status: TaskStatus.DONE,
        userId: mockedUser2.id,
        projectId: mockedProject3.id
    },
    {
        id: 48,
        name: "Conduct user feedback sessions",
        description: "Engage users in feedback sessions to gather insights and suggestions for improving the product.",
        status: TaskStatus.TODO,
        userId: mockedUser3.id,
        projectId: mockedProject4.id
    },
    {
        id: 49,
        name: "Perform code review and optimization",
        description: "Review and optimize the existing codebase to improve performance, readability, and maintainability.",
        status: TaskStatus.IN_PROGRESS,
        userId: mockedUserMain.id,
        projectId: mockedProject1.id
    },
    {
        id: 50,
        name: "Conduct competitor analysis",
        description: "Research and analyze competitors to identify their strengths, weaknesses, and market positioning.",
        status: TaskStatus.DONE,
        userId: mockedUser2.id,
        projectId: mockedProject2.id
    },
    {
        id: 51,
        name: "Finalize project documentation",
        description: "Review and finalize all project documentation, including requirements, design, and user manuals.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUser3.id,
        projectId: mockedProject3.id
    },
    {
        id: 52,
        name: "Perform system backup",
        description: "Execute a full system backup to ensure data integrity and disaster recovery capabilities.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUser2.id,
        projectId: mockedProject4.id
    },
    {
        id: 53,
        name: "Prepare financial reports",
        description: "Compile and analyze financial data to create accurate and comprehensive financial reports.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUser3.id,
        projectId: mockedProject1.id
    },
    {
        id: 54,
        name: "Conduct post-mortem analysis",
        description: "Perform a post-mortem analysis of the project to identify lessons learned and areas for improvement.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUserMain.id,
        projectId: mockedProject2.id
    },
    {
        id: 55,
        name: "Update software licenses",
        description: "Review and renew software licenses to ensure compliance and uninterrupted usage.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUser2.id,
        projectId: mockedProject3.id
    },
    {
        id: 56,
        name: "Prepare quarterly sales report",
        description: "Compile sales data and generate a comprehensive report on quarterly sales performance.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUser3.id,
        projectId: mockedProject4.id
    },
    {
        id: 57,
        name: "Conduct employee satisfaction survey",
        description: "Administer a survey to measure employee satisfaction and identify areas for improvement.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUserMain.id,
        projectId: mockedProject1.id
    },
    {
        id: 58,
        name: "Review and update privacy policy",
        description: "Review the privacy policy and make necessary updates to comply with data protection regulations.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUser2.id,
        projectId: mockedProject2.id
    },
    {
        id: 59,
        name: "Conduct market research",
        description: "Gather data and insights about the target market, industry trends, and customer preferences.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUser3.id,
        projectId: mockedProject3.id
    },
    {
        id: 60,
        name: "Perform system maintenance",
        description: "Execute routine maintenance tasks to ensure the system\"s optimal performance and stability.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUser2.id,
        projectId: mockedProject4.id
    },
    {
        id: 61,
        name: "Update company website content",
        description: "Review and update the content on the company website to reflect the latest information and offerings.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUserMain.id,
        projectId: mockedProject1.id
    },
    {
        id: 62,
        name: "Conduct product demo sessions",
        description: "Organize and deliver product demonstration sessions to showcase the features and benefits of the product.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUserMain.id,
        projectId: mockedProject2.id
    },
    {
        id: 63,
        name: "Prepare annual budget proposal",
        description: "Analyze financial data and prepare a comprehensive budget proposal for the upcoming fiscal year.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUser2.id,
        projectId: mockedProject3.id
    },
    {
        id: 64,
        name: "Review and update software documentation",
        description: "Review and update the software documentation to reflect the latest features and functionalities.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUser2.id,
        projectId: mockedProject4.id
    },
    {
        id: 65,
        name: "Conduct training sessions for new employees",
        description: "Organize and deliver training sessions to onboard and train new employees.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUser3.id,
        projectId: mockedProject1.id
    },
    {
        id: 66,
        name: "Perform competitor analysis",
        description: "Analyze competitors\" products, strategies, and market positioning to identify opportunities and threats.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUserMain.id,
        projectId: mockedProject2.id
    },
    {
        id: 67,
        name: "Prepare investor presentation",
        description: "Create a compelling presentation to showcase the company\"s performance and growth potential to investors.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUser2.id,
        projectId: mockedProject3.id
    },
    {
        id: 68,
        name: "Conduct product quality testing",
        description: "Perform comprehensive testing to ensure the product meets quality standards and specifications.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUser3.id,
        projectId: mockedProject4.id
    },
    {
        id: 69,
        name: "Prepare project closure report",
        description: "Compile a report summarizing the project\"s outcomes, lessons learned, and recommendations for future projects.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUserMain.id,
        projectId: mockedProject1.id
    },
    {
        id: 70,
        name: "Conduct user training sessions",
        description: "Organize and deliver training sessions to educate users on how to effectively use the product or software.",
        status: TaskStatus.ARCHIVED,
        userId: mockedUser2.id,
        projectId: mockedProject2.id
    }
] as Task[];


export function taskById(id: number) {
    return mockedTasks.filter(task => task.id === id)[0]
}




/* 
TODO:

1. edycja menu:
```
Tasks:
    - project1
    - project2
    - ...

All Projects
Users
```
Taski każdego rpojektu wyświetlane oddzielnie

2. Stworzyć Wyuwany Side Panel?
używany zamiast okien modalnych

3. Zmniejszyć taski - są za duże

4. Kanban:
    - TODO
    - IN_PROGRESS
    - DONE

Musi być dostępna opcja wyświetlania tasków *w listach* lub na *Kanban boardzie*
opcja powinna być dostępna do wybrania na panelu do sortowania tak jak inne 
    ustawienia jak wyświetlanie zarchiwizowanych tasków

5. Cacheowanie ustawień

*/