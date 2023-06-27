import {
  createHashRouter, Outlet
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



const AppRouter = createHashRouter([
  {
    path: "/",
    element: <SessionController element={<Outlet />} />,
    errorElement: null,
    children: [
      {
        index: true,
        element: <Login />
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
            path: 'users/:userId/tasks',
            element: <Tasks />,
            errorElement: <ErrorBaner />,
            children: [
              {
                path: ':taskId',
                element: <TaskPanel />
              }
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
                errorElement: <ErrorBaner />,
                path: ':projectId',
                element: <ProjectPanel />,
              },
            ]
          },
          {
            path: 'projects/:projectId/tasks',
            element: <Tasks />,
            errorElement: <ErrorBaner />,
            children: [
              {
                path: ':taskId',
                element: <TaskPanel />,
              }
            ]
          },
        ]
      },
    ]
  },
], {
  basename: process.env.REACT_APP_BASE_URL
});

export default AppRouter;