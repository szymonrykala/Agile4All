import { mockedUserMain, mockedUser2, mockedUser3 } from "./user"


export const mockedProject1 = {
    id: 111,
    name: "EcoTrack Solution",
    description: "EcoTrack is an innovative sustainability monitoring system designed to empower individuals and businesses to track and reduce their environmental footprint. By integrating real-time data collection, intelligent analytics, and user-friendly interfaces, EcoTrack enables users to monitor their energy consumption, waste generation, water usage, and carbon emissions.",
    users: [
        mockedUserMain,
        mockedUser2,
        mockedUser3
    ]
}

export const mockedProject2 = {
    id: 112,
    name: "NexusNet",
    description: "NexusNet is a cutting-edge networking platform designed to revolutionize global connectivity. By integrating advanced satellite technology, mesh networks, and artificial intelligence, NexusNet aims to bridge the digital divide and provide reliable, high-speed internet access to underserved communities worldwide.",
    users: [
        mockedUserMain,
        mockedUser2,
        mockedUser3
    ]
}

export const mockedProject3 = {
    id: 113,
    name: "MindForge",
    description: "MindForge is a cutting-edge brain-computer interface (BCI) project that aims to revolutionize human-computer interaction. By leveraging advanced neurotechnology, MindForge allows users to control digital devices and applications using their thoughts alone. Through the use of non-invasive sensors and sophisticated machine learning algorithms, this groundbreaking project opens up new possibilities for individuals with physical disabilities, enabling them to communicate, navigate, and interact with the digital world effortlessly.",
    users: [
        mockedUser2,
        mockedUser3
    ]
}

export const mockedProject4 = {
    id: 114,
    name: "SolarVista",
    description: "SolarVista is a comprehensive solar energy management system designed to optimize solar panel efficiency and maximize energy production. By integrating state-of-the-art monitoring tools, advanced data analytics, and smart algorithms, SolarVista enables users to monitor the performance of their solar installations in real-time. ",
    users: [
        mockedUser2,
        mockedUser3
    ]
}

export const mockedProjects = [
    mockedProject1,
    mockedProject2,
    mockedProject3,
    mockedProject4
]

export const mockedUserProjects = [
    mockedProject1,
    mockedProject2
]


export function projectById(id: number) {
    return mockedProjects.filter(proj => proj.id === id)[0]
}