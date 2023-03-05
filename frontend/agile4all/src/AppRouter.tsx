import {
  createHashRouter, Outlet
} from "react-router-dom";
import App from "./App";
import Projects from "./components/Pages/Projects";
import Users from "./components/Pages/Users";
import Tasks from "./components/Pages/Tasks";
import TaskModal from "./components/Tasks/TaskModal";
import Login from "./components/Pages/Login";
import Registration from "./components/Pages/Registration";
import SessionController from "./components/Pages/SessionControler";
import Logout from "./components/Pages/Logout";
import UserModal from "./components/Users/UserModal";
import { ErrorBaner } from "./components/Errors";
import ProjectModal from "./components/Projects/ProjectModal";



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
                element: <TaskModal />
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
                element: <UserModal />,
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
                element: <ProjectModal />,
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
                element: <TaskModal />,
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