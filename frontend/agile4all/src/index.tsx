import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import { CssVarsProvider, StyledEngineProvider } from '@mui/joy/styles';
import store from './store'
import { Provider } from 'react-redux'
import teamTheme from './theme';
import { GlobalStyles } from '@mui/system';
import type { Theme } from '@mui/joy/styles';
import {
  RouterProvider,
} from "react-router-dom";
import AppRouter from './AppRouter';
import { ReloadTriggerContextProvider } from './components/common/ReloadTrigger';
import NotificationContextProvider from './components/Notification/Context';



const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);



root.render(
  <React.StrictMode>
    <StyledEngineProvider injectFirst>
      <CssVarsProvider disableTransitionOnChange theme={teamTheme}>
        <GlobalStyles<Theme>
          styles={(theme) => ({
            body: {
              margin: 0,
              fontFamily: theme.vars.fontFamily.body,
            },
          })}
        />
        <Provider store={store}>
          <ReloadTriggerContextProvider>
            <NotificationContextProvider>
              <RouterProvider router={AppRouter} />
            </NotificationContextProvider>
          </ReloadTriggerContextProvider>
        </Provider>
      </CssVarsProvider >
    </StyledEngineProvider>
  </React.StrictMode>
);
