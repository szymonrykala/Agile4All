import {
  createHashRouter, Navigate, Outlet
} from "react-router-dom";
import App from "./App";
import Projects from "./components/Pages/Projects";
import Users from "./components/Pages/Users";
import Tasks from "./components/Pages/Tasks";
import TaskPanel from "./components/Tasks/TaskPanel";
import Login from "./components/Pages/Login";
import Registration from "./components/Pages/Registration";
import SessionController from "./components/Pages/SessionControler";
import Logout from "./components/Pages/Logout";
import UserPanel from "./components/Users/UserPanel";
import { ErrorBaner } from "./components/Errors";
import ProjectPanel from "./components/Projects/ProjectPanel";


const SIDE_PANEL_LOOKUPS = [
  {
    path: 'task-lookup/:taskId',
    element: <TaskPanel />
  },
  {
    path: 'project-lookup/:projectId',
    element: <ProjectPanel />
  }
]


const AppRouter = createHashRouter([
  {
    path: "/",
    element: <SessionController element={<Outlet />} />,
    errorElement: null,
    children: [
      {
        index: true,
        element: <Navigate to="/app" />
      },
      {
        path: 'login',
        element: <Login />
      },
      {
        path: 'logout',
        element: <Logout />
      },
      {
        path: 'register',
        element: <Registration />
      },
      {
        path: 'app',
        element: <SessionController element={<App />} />,
        children: [
          {
            path: "tasks",
            errorElement: <ErrorBaner />,
            children: [
              {
                path: "users/:userId/projects/:projectId",
                element: <Tasks />,
                errorElement: <ErrorBaner />,
                children: SIDE_PANEL_LOOKUPS
              },
              {
                path: "users/:userId",
                element: <Tasks />,
                errorElement: <ErrorBaner />,
                children: SIDE_PANEL_LOOKUPS
              },
              {
                path: "projects/:projectId",
                element: <Tasks />,
                errorElement: <ErrorBaner />,
                children: SIDE_PANEL_LOOKUPS
              },
            ]
          },
          {
            path: 'users',
            element: <Users />,
            errorElement: <ErrorBaner />,
            children: [
              {
                path: ':userId',
                element: <UserPanel />,
              }
            ]
          },
          {
            path: 'projects',
            element: <Projects />,
            children: [
              {
                path: ':projectId',
                element: <ProjectPanel />,
                errorElement: <ErrorBaner />,
              },
              {
                path: ":projectId/task-lookup/:taskId",
                element: <TaskPanel/>,
                errorElement: <ErrorBaner />,
              },
              {
                path: "user-lookup/:userId",
                element: <UserPanel/>,
                errorElement: <ErrorBaner />,
              }
            ]
          }
        ]
      }
    ]
  },
], {
  basename: process.env.REACT_APP_BASE_URL
});

export default AppRouter;